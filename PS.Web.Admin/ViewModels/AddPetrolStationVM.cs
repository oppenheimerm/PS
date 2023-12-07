using PS.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace PS.Web.Admin.ViewModels
{
    public class AddPetrolStationVM
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string StationName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "Address")]
        public string StationAddress { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Address2")]
        public string? StationAddress2 { get; set; } = string.Empty;

        [Required]
        [StringLength(25)]
        [Display(Name = "Postcode")]
        public string StationPostcode { get; set; } = string.Empty;

        [Required]
        public double? Latitude { get; set; } 

        [Required]
        public double? Longitude { get; set; }

        [Required]
        public int? VendorId { get; set; }

        [Required]
        public int? CountryId { get; set; }

        public string? CountryCode { get; set; }

        //  Drop down list
        public List<Country>? Countries { get; set; }
        public List<Vendor>? Vendors { get; set;}
        public bool PayAtPump { get; set; } = false;
        public bool PayByApp { get; set; } = false;
        public bool AccessibleToiletNearby { get; set; } = false;
    }
}
