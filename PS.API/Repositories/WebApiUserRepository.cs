using Microsoft.EntityFrameworkCore;
using PS.API.Authorization.Interfaces;
using PS.API.Repositories.Interfaces;
using PS.Core.Models;
using PS.Datastore.EFCore;
using PS.API.ModelHelpers;
using PS.Core.Helpers;
using BC = BCrypt.Net.BCrypt;
using PS.Core.Models.ApiRequestResponse;
using System.Net;
using PS.API.Helpers;


namespace PS.API.Repositories
{
	/*
     * TODO
     *  ask RegisterAsync(RegisterRequest model, string origin)
     *      - https://www.tutorialspoint.com/Chash-program-to-check-the-validity-of-a-Password
     *  account vefification
     *      - account.Verified = DateTime.UtcNow;
     *  send email
     *      sendVerificationEmail(account, origin);
     *      
     * 
     */
	public class WebApiUserRepository : IWebApiUserRepository
    {
        private readonly ApplicationDbContext Context;
        private readonly IJwtUtils JwtUtilis;
        private readonly IConfiguration Configuration;
        private readonly ILogger<WebApiUserRepository> Logger;
        private IWebHostEnvironment Environment;
        private readonly IPhotoFileRepository PhotoFileRepository;

        public WebApiUserRepository(ApplicationDbContext context,
            IJwtUtils jwtUtilis,
            IConfiguration configuration,
            IPhotoFileRepository photoFileRepository,
            ILogger<WebApiUserRepository> logger,
            IWebHostEnvironment environment)
        {
            Context = context;
            JwtUtilis = jwtUtilis;
            Configuration = configuration;
            Logger = logger;
            Environment = environment;
            PhotoFileRepository = photoFileRepository;
        }

        public async Task<(bool Success, string ErrorMessage)> RegisterAsync(RegisterRequest model, string origin)
        {
            var memberExist = await Context.Members.SingleOrDefaultAsync(m => m.EmailAddress == model.EmailAddress.ToLower());
            // validate
            if (Context.Members.Any(x => x.EmailAddress == model.EmailAddress.ToLowerInvariant()))
            {
				//  TODO
				// send already registered error in email to prevent account enumeration
				//sendAlreadyRegisteredEmail(model.Email, origin);
				//  See: sendAlreadyRegisteredEmail(string email, string origin)
				return (false, "User already registered");
            }

            if(Context.Members.Any(x => x.MobileNumber == model.MobileNumber.ToLowerInvariant()))
            {
                return (false, "This mobible phone is already in use.");
            }

            //  Must accept terms
            if(model.AcceptTerms == false)
            {
                return (false, "You must agree to terms in order to register with PetrolSist");
            }

            // map model to new account object
            var account = model.ToMember();


            account.Created = DateTime.UtcNow;
            var randomString = new RandomStringGenerator();
            account.VerificationToken = randomString.Generate(13);

            account.MemberPhoto = string.Empty;

            // TODO
            //  REMOVE FOR PRODUCTION
            //account.Verified = DateTime.UtcNow;

            //  TODO https://www.tutorialspoint.com/Chash-program-to-check-the-validity-of-a-Password

            // hash password
            account.PasswordHash = BC.HashPassword(model.Password);

            // save account
            await Context.Members.AddAsync(account);
            await Context.SaveChangesAsync();
            Logger.LogInformation($"Sucessfully added new user: {model.EmailAddress} at: {DateTime.UtcNow}");
            //  TODO
            // send email
            //sendVerificationEmail(account, origin);

            return (true, string.Empty);
        }

        public async Task<AuthenticateResponse> LogInAsync(AuthenticateRequest model, string ipAddress)
        {
            var errorMessage = string.Empty;
            var rsp = new AuthenticateResponse();
            var user = await Context.Members.SingleOrDefaultAsync(m => m.EmailAddress == model.EmailAddress.ToLowerInvariant());
            if(user == null)
            {
                errorMessage = "User not found";
                Logger.LogError($"Failed login attempt failed for Email: {model.EmailAddress}. Timestamp : {DateTime.UtcNow}");
                rsp.StatusCode = (int)HttpStatusCode.NotFound;
                rsp.Message = errorMessage;
                return rsp;
            }

            //  Verify that user has verified their email address
            if(user.Verified == null)
            {
                //  Account not verified
                errorMessage = "Please verify your eamail address";
                Logger.LogError($" User {user.EmailAddress} attempted to login without email verfication. Timestamp : {DateTime.UtcNow}");
                rsp.StatusCode = (int)HttpStatusCode.NotFound;
                rsp.Message = errorMessage;
                return rsp;
            }

            // validate
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                Logger.LogError($"Failed login attempt failed for Email: {model.EmailAddress}. Timestamp : {DateTime.UtcNow}");
                //throw new AppException("Email or password is incorrect");                
                errorMessage = "Email or password is incorrect";
                rsp.StatusCode = (int)HttpStatusCode.Unauthorized;
                rsp.Message = errorMessage;
                return rsp;
            };                               
           
                

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = JwtUtilis.GenerateJwtToken(user);
            var refreshToken = await JwtUtilis.GenerateRefreshTokenAsync(ipAddress);
            user?.RefreshTokens?.Add(refreshToken);
            user.LastLogIn = DateTime.UtcNow;

            // remove old refresh tokens from user
            removeOldRefreshTokens(user);

            Context.Update(user);
            await Context.SaveChangesAsync();
            Logger.LogInformation($"User: {model.EmailAddress}, login at {DateTime.UtcNow}");


            //return new AuthenticateResponse(user, jwtToken, refreshToken.Token, errorMessage, (int)HttpStatusCode.OK);

            //  TODO - Extract below into a function
            /*rsp.StatusCode = (int)HttpStatusCode.OK;
            rsp.Id = user.Id;
            rsp.FirstName = user.FirstName;
            rsp.LastName = user.LasttName;
            rsp.JwtToken = jwtToken;
            rsp.Initials = user.Initials;
            rsp.Photo = "PHOTO";
            rsp.EmailAddress = user.EmailAddress;
            rsp.StatusCode = (int)HttpStatusCode.OK;
            rsp.RefreshToken = refreshToken.Token;
            rsp.Message = string.Empty;
            rsp.MobileNumber = user.MobileNumber;
            return rsp;*/

            var userResponse = GetAuthenticatedResponse(
                user,
				jwtToken,
				refreshToken.Token,
				(int)HttpStatusCode.OK
                );

            return userResponse;

		}

        public async Task<Member>GetMemberByIdAsync(Guid id)
        {
            var user = await Context.Members.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null) throw new KeyNotFoundException("User not found");

            return user;
        }

        public async Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress)
        {
            var response = await GetUserByRefreshTokenAsync(token);
            if (response.Success) {
                var refreshToken = response.user?.RefreshTokens?.Single(x => x.Token == token);

                if (refreshToken.IsRevoked)
                {
                    // revoke all descendant tokens in case this token has been compromised
                    revokeDescendantRefreshTokens(refreshToken, response.user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                    Context.Update(response.user);
                    await Context.SaveChangesAsync();
                }

                if (!refreshToken.IsActive)
                    Logger.LogError($"Invalid Toke Exception raised at: {DateTime.UtcNow}");

                // replace old refresh token with a new one (rotate token)
                var newRefreshToken = await RotateRefreshTokenAsync(refreshToken, ipAddress);
                response.user.RefreshTokens.Add(newRefreshToken);

                // remove old refresh tokens from user
                removeOldRefreshTokens(response.user);

                // save changes to db
                Context.Update(response.user);
                await Context.SaveChangesAsync();

                // generate new jwt
                var jwtToken = JwtUtilis.GenerateJwtToken(response.user);
                var authResponse = GetAuthenticatedResponse(response.user, jwtToken, newRefreshToken.Token, (int)HttpStatusCode.OK);

                return authResponse;
            }
            else
            {
                var responeError = new AuthenticateResponse()
                {
                    Message = response.ErrorMessage,
                    StatusCode = 404
                };
                return responeError;
            }
        }


        //  Our test function to mock getting user profile



        public async Task<(bool Success, string ErrorMessage)> VerifyEmailAsync(string token)
		{
            if (token == null)
            {
                var error = $"Faild validation no token found. TimeStamp: {DateTime.UtcNow}";
				Logger.LogError(error);
                return (false, $"Faild validation no token found. TimeStamp: {DateTime.UtcNow}");
            }

			var account = await Context.Members.SingleOrDefaultAsync(x => x.VerificationToken == token);

            if (account == null)
            {
                var error = $"Account verification failed. TimeStamp: {DateTime.UtcNow}";
				Logger.LogError(error);
                return(false, error);
			}

			account.Verified = DateTime.UtcNow;
			account.VerificationToken = string.Empty;

			Context.Members.Update(account);
			await Context.SaveChangesAsync();
            var msg = $"Succesfuly validated account. TimeStamp: {DateTime.UtcNow}";
            return (true, msg);
		}

        //  TODO - Change response type
        //  This will cause a bottle-neck so we will pass this out to a queue
        public async Task<UploadPhotoResponse> UploadUPhoto(string token, IFormFile imageFile)
        {
            var response = await GetUserByRefreshTokenAsync(token);
            if (response.Success)
            {
                //folderEntity.AbsolutePath = Path.Combine(Environment.WebRootPath, Core.Helpers.Constants.RootDirectory, folderStatus.directoryInfo.Name);
                var absolutePath = Path.Combine(Environment.WebRootPath, Helpers.Constants.RootDirectory);
                // Is valid file?
                if (FileHelpers.ValidImageFile(imageFile))
                {
                    //var addPhoto = await PhotoFileRepository.AddPhotoAsync(NewPhoto.Photo, folderDbEntity.folderDBEntity.FolderName)
                    var addPhoto = await PhotoFileRepository.AddPhotoAsync(imageFile, absolutePath);
                    if (addPhoto.Success)
                    {
                        return new UploadPhotoResponse()
                        {
                            StatusCode = 200,
                            Message = string.Empty,
                            FileName = addPhoto.fileInfo.Name,  
                            Member = response.user
                        };
                    }
                    else {
                        // faild to add photo for some reasnon
                        return new UploadPhotoResponse()
                        {
                            StatusCode = 500,
                            Message = addPhoto.ErrorMessage,
                            FileName = string.Empty
                        };
                    }
                }
                else {
                    //  Invalid file type
                    return new UploadPhotoResponse()
                    {
                        StatusCode = 500,
                        Message = "Invalid photo file extension",
                        FileName = string.Empty
                    };
                }
            }
            else
            {
                return new UploadPhotoResponse()
                {
                    StatusCode = 404,
                    Message = "User not found",
                    FileName = string.Empty
                };
            }
        }



		#region helpers

        /// <summary>
        /// Helper method to populate a <see cref="AuthenticateResponse"/> with instance
        /// data of an authticate <see cref="Member"/>(user)
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="jwtToken"></param>
        /// <param name="photo"></param>
        /// <param name="emailAddress"></param>
        /// <param name="refreshToken"></param>
        /// <param name="errorMessage"></param>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        /*public AuthenticateResponse GetUserResponse(
            int statusCode,
            Guid id,
            string firstName,
            string lastName,
            string jwtToken,
            string photo,
            string emailAddress,
            string refreshToken,
            string errorMessage,
            string mobileNumber
            ) {
            var response = new AuthenticateResponse()
            {
                StatusCode = statusCode,
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                JwtToken = jwtToken,
                Photo = photo,
                EmailAddress = emailAddress,
                RefreshToken = refreshToken,
                Message = errorMessage,
                MobileNumber = mobileNumber
            };

            return response;

		}*/

        private async Task<(Member user, bool Success, string ErrorMessage)> GetUserByRefreshTokenAsync(string token)
        {
            var user = await Context.Members.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                var errMsg = $"Token inavlid {token}!.  TimeStamp: {DateTime.UtcNow}";
                Logger.LogError(errMsg);
                return (new Member(), false, errMsg);
            }

            return (user, true, string.Empty);
        }

        private void removeOldRefreshTokens(Member user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(int.Parse(Configuration["WebApiSecurity:RefreshTokenTTL"])) <= DateTime.UtcNow);
        }

        private async Task<Member> getUserByRefreshTokenAsync(string token)
        {
            var user = await Context.Members.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                Logger.LogError($"Invalid Toke Exception raised at: {DateTime.UtcNow}");

            return user;
        }

        private void revokeDescendantRefreshTokens(RefreshToken refreshToken, Member user, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens?.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                    revokeRefreshToken(childToken, ipAddress, reason);
                else
                    revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
            }
        }

        private void revokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null, string? replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
            Logger.LogInformation($"Token {token.Token} revoked at {DateTime.UtcNow}. Reason: {token.ReasonRevoked}");
        }

        private async Task<RefreshToken> RotateRefreshTokenAsync(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = await JwtUtilis.GenerateRefreshTokenAsync(ipAddress);
            revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }
        private AuthenticateResponse GetAuthenticatedResponse(Member user, string jwtToken, string newRefreshToken, int statusCode)
        {
            return new AuthenticateResponse()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LasttName,
                JwtToken = jwtToken,
                Initials = user.Initials,
                Photo = user.MemberPhoto,
                EmailAddress = user.EmailAddress,
                RefreshToken = newRefreshToken,
                Message = string.Empty,
                StatusCode = statusCode,
                MobileNumber = user.MobileNumber,
                DistanceUnit = user.DistanceUnit,
            };
        }

        //  TODO - Add MailService
		/*private void sendAlreadyRegisteredEmail(string email, string origin)
		{
			string message;
			if (!string.IsNullOrEmpty(origin))
				message = $@"<p>If you don't know your password please visit the <a href=""{origin}/account/forgot-password"">forgot password</a> page.</p>";
			else
				message = "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>";

			_emailService.Send(
				to: email,
				subject: "Sign-up Verification API - Email Already Registered",
				html: $@"<h4>Email Already Registered</h4>
                        <p>Your email <strong>{email}</strong> is already registered.</p>
                        {message}"
			);
		}*/

		#endregion
	}
}

