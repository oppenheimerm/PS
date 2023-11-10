using PS.Core.Models.ApiRequestResponse;
using PS.Core.Models;

namespace PS.API.Repositories.Interfaces
{
    public interface IWebApiUserRepository
    {
        /// <summary>
        /// User (<see cref="Member"/>) Login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        Task<AuthenticateResponse> LogInAsync(AuthenticateRequest model, string ipAddress);
        /// <summary>
        /// Register a new User(<see cref="Member"/>).
        /// </summary>
        /// <param name="model"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        Task<(bool Success, string ErrorMessage)> RegisterAsync(RegisterRequest model, string origin);
		/// <summary>
		/// Return a <see cref="Member"/> by Id(Guid)
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<Member> GetMemberByIdAsync(Guid id);
        /// <summary>
        /// Refresh token for an authenticated <see cref="Member"/>.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress);
        /// <summary>
        /// Email / Account verification for a new <see cref="Member"/>
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<(bool Success, string ErrorMessage)> VerifyEmailAsync(string token);
		//Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress);
		//Task RevokeTokenAsync(string token, string ipAddress);

	}
}
