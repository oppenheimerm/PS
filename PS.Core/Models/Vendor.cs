
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PS.Core.Models
{
    public class Vendor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string VendorName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "Address")]
        public string VendorAddress { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Address2")]
        public string? VendorAddress2 { get; set; } = string.Empty;
        
        [StringLength(200)]
        [Display(Name = "Address3")]
        public string? VendorAddress3 { get; set; } = string.Empty;


		[Required]
        [StringLength(25)]
        [Display(Name = "Postcode")]
        public string VendorPostcode { get; set; } = string.Empty;


        [Required]
        [ForeignKey("Country")]
        public int? CountryId { get; set; }
        public Country? Country { get; set; }

        [StringLength(15)]
        public string Logo { get; set; } = string.Empty;

        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string VendorCode { get; set; } = string.Empty;

        public ICollection<Station>? Stations { get; set; }
    }
}
