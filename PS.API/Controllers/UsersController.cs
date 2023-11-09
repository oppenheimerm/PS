using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PS.API.Repositories.Interfaces;
using PS.Core.Models.ApiRequestResponse;
using System.Net;

namespace PS.API.Controllers
{
    //  Base account controller: https://github.com/cornflourblue/aspnet-core-3-signup-verification-api/blob/master/Controllers/BaseController.cs
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IWebApiUserRepository WebApiUserRepository;

        public UsersController(IWebApiUserRepository webApiUserRepository)
        {
            WebApiUserRepository = webApiUserRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await WebApiUserRepository.LogInAsync(model, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }
        

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthenticateResponse>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await WebApiUserRepository.RefreshTokenAsync(refreshToken, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
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
        /// new token from the <see cref="UsersController.RefreshToken"/> route which prevents 
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
