
using System.ComponentModel.DataAnnotations;

namespace PS.Core.Models.ApiRequestResponse
{
	public class VerifyEmailRequest
	{
		[Required]
		public string Token { get; set; } = string.Empty;
	}
}
