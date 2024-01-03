using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PS.API.Repositories.Interfaces;
using PS.Core.Models.ApiRequestResponse;
using System.Net;

namespace PS.API.Controllers.V1
{
    //  Base account controller: https://github.com/cornflourblue/aspnet-core-3-signup-verification-api/blob/master/Controllers/BaseController.cs
    //
    //  URL based Versioning
    // Versioning call as normal or: https://localhost:44381/api/users?api-version=1.0
    [Authorization.Attributes.Authorize]
    [ApiVersion("1.0")]
	[Route("api/{v:apiVersion}/users")]
	//[Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IWebApiUserRepository WebApiUserRepository;
        private readonly IMemberRepository MembersRepository;

        public UsersController(IWebApiUserRepository webApiUserRepository, IMemberRepository memberRepository)
        {
            WebApiUserRepository = webApiUserRepository;
            MembersRepository = memberRepository;
        }

        /// <summary>
        /// Register as a user on the PetrolSist application.  It takes a <see cref="RegisterRequest"/>
        /// parameter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest model)
        {
            var requestStatus = await WebApiUserRepository.RegisterAsync(model, Request.Headers["origin"]);
            if (requestStatus.Success)
            {
                return Ok(new { message = "Registration successful, please check your email for verification instructions" });
            }
            else
            {
                return BadRequest(new { message = requestStatus.ErrorMessage });
            }

        }

        /// <summary>
        /// On registering, a user must verfiy their email is an valid email address.  This method
        /// takes a <see cref="VerifyEmailRequest"/> parameter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest model)
        {
            var requestStatus = await WebApiUserRepository.VerifyEmailAsync(model.Token);
            if (requestStatus.Success)
            {
                return Ok(new { message = "Verification successful, you can now login" });
            }
            else
            {
                return BadRequest(new { message = requestStatus.ErrorMessage });
            }
        }

        /// <summary>
        /// This the method to call to login a <see cref="VerifyEmailRequest"/> account.
        /// It takes a <see cref="AuthenticateRequest"/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await WebApiUserRepository.LogInAsync(model, ipAddress());
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                setTokenCookie(response.RefreshToken);
                return Ok(response);
            }
            else
            {
                return Unauthorized(response);
            }

        }

		/// <summary>
		/// Method to handle user's refresh token request.  This takes
		/// a httpCookie with the refresh token in the header after a user
		/// has been authenticated with the <see cref="Authenticate"/>
        /// method
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthenticateResponse>> RefreshToken()
        {

            var refreshToken = Request.Cookies["refreshToken"];
            var response = await WebApiUserRepository.RefreshTokenAsync(refreshToken, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }


        /*[HttpGet("profile")]
        public async Task<ActionResult<AuthenticateResponse>> GetProfile()
        {
            // Check for null
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await WebApiUserRepository.GetUser(refreshToken, string.Empty);
            return Ok(response);
        }*/

        [HttpPost("upload-photo"), DisableRequestSizeLimit]
        public async Task<ActionResult<UploadPhotoResponse>> UplaodPhoto(IFormFile file)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
            {
                return new UploadPhotoResponse()
                {
                    StatusCode = 401,
                    Message = "Not Authorized",
                    FileName = string.Empty
                };
            }
            else
            {
                var fileFormCollection = await Request.ReadFormAsync();
                var fileToUpload = fileFormCollection.Files.First();

                if(fileToUpload == null)
                {
                    return new UploadPhotoResponse()
                    {
                        StatusCode = 404,
                        Message = "Photo missing",
                        FileName = string.Empty
                    };
                }
                else
                {
                    var response = await WebApiUserRepository.UploadUPhoto(refreshToken, fileToUpload);
                    if(response.StatusCode == 200  && response.Member != null)
                    {
                        //  Good to go, update profile data
                        var updateUserPhotoStatus = await MembersRepository.UpdateUserPhoto(response.Member, response.FileName);
                        if(updateUserPhotoStatus.Success)
                        {
                            response.Member.MemberPhoto = response.FileName;
                            return Ok(response); 
                        }
                        else
                        {
                            //   unable to update member photo for some resaon, return friendl error message,
                            //  error would have already been logged ny the calling function above
                            return NotFound(response);

                        }

                    }
                    else{
                        //  Just return the response which willl inlcude failed message
                        return response;
                    }                    
                }
            }
            
        }

        #region Helpers


        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        /// <summary>
        /// Helper method appends an HTTP Only cookie containing the refresh token to the response for 
        /// increased security. HTTP Only cookies are not accessible to client-side javascript which 
        /// prevents XSS (cross site scripting), and the refresh token can only be used to fetch a 
        /// new token from the <see cref="RefreshToken"/> route which prevents 
        /// CSRF (cross site request forgery).
        /// </summary>
        /// <param name="token"></param>
        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        #endregion
    }
}
