
using System.Text.Json.Serialization;

namespace PA.Core.Models.ApiRequestResponse
{
	public class AuthenticateResponse : BaseResponse
	{
		public Guid Id { get; set; } = Guid.Empty;
		public string? FirstName { get; set; } = string.Empty;
		public string? LastName { get; set; } = string.Empty;
		//public string? Username { get; set; } = string.Empty;
		public string? JwtToken { get; set; } = string.Empty;
		public string? Initials { get; set; } = string.Empty;
		public string? Photo { get; set; } = string.Empty;
		public string? EmailAddress { get; set; } = string.Empty;
		public string? MobileNumber { get; set; } = string.Empty;
		public int DistanceUnit { get; set; } = 1;

		[JsonIgnore] // refresh token is returned in http only cookie
		public string RefreshToken { get; set; } = string.Empty;
	}
}
