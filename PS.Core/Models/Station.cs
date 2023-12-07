
using System.ComponentModel.DataAnnotations;

namespace PS.Core.Models
{
    public class Station
    {
        public Station()
        {
            this.Added = DateTime.Now;
            this.StationIdentifier = Guid.NewGuid();
        }

        [Key]
        public int Id { get; set; }

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

        public bool StationOnline { get; set; } = false;

        [Required]
        public int? VendorId { get; set; }
        public Vendor? Vendor { get; set; }

        [Required]
        public int? CountryId { get; set; }

        [Required]
        [MaxLength(3, ErrorMessage = "Country code must be 3 characters long"), MinLength(3)]
        public string? CountryCode { get; set; } = string.Empty;

        public DateTime? Added { get; set; }

        public Guid StationIdentifier { get; private set; }

        public bool PayAtPump { get; set; } = false;
        public bool PayByApp { get; set; } = false;

        public bool AccessibleToiletNearby { get; set; } = false;
    }
}
