
using System.ComponentModel.DataAnnotations;

namespace PA.Core.Models.ApiRequestResponse
{
	public class AuthenticateRequest
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string EmailAddress { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;
	}
}
