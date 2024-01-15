
using System.ComponentModel.DataAnnotations;

namespace PA.Core.Models.ApiRequestResponse
{
	public class VerifyEmailRequest
	{
		[Required]
		public string Token { get; set; } = string.Empty;
	}
}
