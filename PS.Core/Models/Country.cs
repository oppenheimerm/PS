

using System.ComponentModel.DataAnnotations;

namespace PS.Core.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Country")]
        public string CountryName { get; set; } = string.Empty;

        [Required]
        [MaxLength(3, ErrorMessage = "Country code must be 3 characters long"), MinLength(3)]
        public string? CountryCode { get; set; } = string.Empty;
    }
}
