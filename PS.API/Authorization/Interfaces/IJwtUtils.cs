using PS.Core.Models;

namespace PS.API.Authorization.Interfaces
{
    public interface IJwtUtils
    {
        string GenerateJwtToken(Member user);
        Guid? ValidateJwtToken(string token);
        public Task<RefreshToken>GenerateRefreshTokenAsync(string ipAddress);
    }
}
