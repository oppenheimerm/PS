﻿using Microsoft.EntityFrameworkCore;
using PS.API.Authorization.Interfaces;
using PS.API.Exceptions;
using PS.API.Repositories.Interfaces;
using PS.Core.Models;
using PS.Datastore.EFCore;
using PS.API.ModelHelpers;
using PS.Core.Helpers;
using BC = BCrypt.Net.BCrypt;
using PS.Core.Models.ApiRequestResponse;
using System.Net;


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

        public WebApiUserRepository(ApplicationDbContext context,
            IJwtUtils jwtUtilis,
            IConfiguration configuration,
            ILogger<WebApiUserRepository> logger)
        {
            Context = context;
            JwtUtilis = jwtUtilis;
            Configuration = configuration;
            Logger = logger;
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

            account.MemberPhoto = "PHOTO";

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
            user.RefreshTokens.Add(refreshToken);
            user.LastLogIn = DateTime.UtcNow;

            // remove old refresh tokens from user
            removeOldRefreshTokens(user);

            Context.Update(user);
            await Context.SaveChangesAsync();
            Logger.LogInformation($"User: {model.EmailAddress}, login at {DateTime.UtcNow}");


            //return new AuthenticateResponse(user, jwtToken, refreshToken.Token, errorMessage, (int)HttpStatusCode.OK);
            rsp.StatusCode = (int)HttpStatusCode.OK;
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
            return rsp;

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
                var authResponse = GetAuthenticatedResponse(response.user, jwtToken, newRefreshToken.Token);

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

		#region helpers
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
        private AuthenticateResponse GetAuthenticatedResponse(Member user, string jwtToken, string newRefreshToken)
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
                StatusCode = 200,
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

