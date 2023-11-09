using System.ComponentModel.DataAnnotations;

namespace PS.Web.Admin.ViewModels
{
    public class AddCountryVM
    {

        [Required]
        [StringLength(100)]
        [Display(Name = "Country")]
        public string CountryName { get; set; } = string.Empty;

        [Required]
        [MaxLength(3, ErrorMessage = "Country code must be 3 characters long"), MinLength(3)]
        public string? CountryCode { get; set; } = string.Empty;
    }
}
