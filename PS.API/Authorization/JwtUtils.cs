using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PS.API.Authorization.Interfaces;
using PS.Core.Models;
using PS.Datastore.EFCore;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PS.API.Authorization
{
    public class JwtUtils : IJwtUtils
    {
        private readonly ApplicationDbContext Context;
        private readonly IConfiguration Configuration;
        private readonly ILogger<JwtUtils> Logger;

        public JwtUtils(ApplicationDbContext context, IConfiguration configuration, ILogger<JwtUtils> logger)
        {
            Context = context;
            Configuration = configuration;
            Logger = logger;
        }

        public string GenerateJwtToken(Member user)
        {
            // generate token that is valid for 15 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["WebApiSecurity:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Guid? ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["WebApiSecurity:Secret"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch(SecurityTokenExpiredException err)
            {
                // return null if validation fails
                Logger.LogError($"Failed to validate JwtToken: {err.ToString()} at {DateTime.UtcNow}");
                return null;
            }
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Token = await getUniqueTokenAsync(),
                // token is valid for 7 days
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            return refreshToken;

            async Task<string> getUniqueTokenAsync()
            {
                // token is a cryptographically strong random sequence of values
                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                // ensure token is unique by checking against db
                var tokenIsUnique = ! await Context.Members.AnyAsync(u => u.RefreshTokens.Any(t => t.Token == token));

                if (!tokenIsUnique)
                    return await getUniqueTokenAsync();

                return token;
            }
        }
    }
}
