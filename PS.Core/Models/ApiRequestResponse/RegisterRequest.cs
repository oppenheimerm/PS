using System.ComponentModel.DataAnnotations;

namespace PS.Core.Models.ApiRequestResponse
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(7)]
        public string Title { get; set; }

        [Required(ErrorMessage = "First name field is required")]
        [StringLength(30)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name field is required")]
        [StringLength(30)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(3)]
        [StringLength(8),]
        public string? Initlals { get; set;}

        [Required(ErrorMessage = "A valid email is required to register")]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [MinLength(7)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Range(typeof(bool), "true", "true", ErrorMessage = "To use this service, you must agree to our terms of service.")]
        public bool AcceptTerms { get; set; } = false;
    }
}
