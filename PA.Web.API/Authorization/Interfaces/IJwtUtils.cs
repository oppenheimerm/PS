using PA.Core.Models;

namespace PA.Web.API.Authorization.Interfaces
{
    public interface IJwtUtils
    {
        string GenerateJwtToken(Member user);
        Guid? ValidateJwtToken(string token);
        public Task<RefreshToken> GenerateRefreshTokenAsync(string ipAddress);
    }
}
