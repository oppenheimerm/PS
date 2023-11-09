using PS.Core.Models.ApiRequestResponse;
using PS.Core.Models;

namespace PS.API.Repositories.Interfaces
{
    public interface IWebApiUserRepository
    {
        Task<AuthenticateResponse> LogInAsync(AuthenticateRequest model, string ipAddress);
        Task RegisterAsync(RegisterRequest model, string origin);
        Task<Member> GetMemberByIdAsync(Guid id);
        Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress);
        //Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress);
        //Task RevokeTokenAsync(string token, string ipAddress);

    }
}
