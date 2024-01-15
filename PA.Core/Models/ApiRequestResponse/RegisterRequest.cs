

using System.ComponentModel.DataAnnotations;

namespace PA.Core.Models.ApiRequestResponse
{
	public class RegisterRequest
	{

		[Required(ErrorMessage = "First name field is required")]
		[StringLength(30)]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Last name field is required")]
		[StringLength(30)]
		public string LastName { get; set; } = string.Empty;

		[Required(ErrorMessage = "A valid email is required to register")]
		[EmailAddress]
		public string EmailAddress { get; set; } = string.Empty;

		[Required]
		[MinLength(7)]
		public string Password { get; set; } = string.Empty;

		[Range(typeof(bool), "true", "true", ErrorMessage = "To use this service, you must agree to our terms of service.")]
		public bool AcceptTerms { get; set; } = false;

		[Required]
		public string MobileNumber { get; set; } = string.Empty;
	}
}
