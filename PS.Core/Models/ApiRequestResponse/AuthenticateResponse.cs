using PS.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PS.Core.Models.ApiRequestResponse
{
    public class AuthenticateResponse : BaseResponse
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        //public string? Username { get; set; } = string.Empty;
        public string? JwtToken { get; set; } = string.Empty;
        public string? Initials { get; set; } = string.Empty;
        public string? Photo {  get; set; } = string.Empty;
        public string? EmailAddress { get; set; } = string.Empty;

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; } = string.Empty;


        /*[JsonConstructor]
        public AuthenticateResponse(
            Guid id, 
            string firstName,
            string lastName,
            string jwtToken,
            string initials,
            string photo,
            string emaiilAddress,
            int statusCode,
            string refreshToken,
            string message
            )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            JwtToken = jwtToken;
            Initials = initials;
            Photo = photo;
            EmailAddress = emaiilAddress;
            StatusCode = statusCode;
            Message = message;
            RefreshToken = refreshToken;
        }*/

    }
}
