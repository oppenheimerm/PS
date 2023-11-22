using PS.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PS.Web.Admin.ViewModels
{
	public class AddVendorVM
	{
		[Required]
		[StringLength(100)]
		[Display(Name = "Name")]
		public string VendorName { get; set; } = string.Empty;

		[Required]
		[StringLength(200)]
		[Display(Name = "Address")]
		public string VendorAddress { get; set; } = string.Empty;


		[StringLength(200)]
		[Display(Name = "Address line 2")]
		public string? VendorAddress2 { get; set; } = string.Empty;

		[StringLength(200)]
		[Display(Name = "Address line 3")]
		public string? VendorAddress3 { get; set; } = string.Empty;

		[Required]
		[StringLength(25)]
		[Display(Name = "Postcode")]
		public string VendorPostcode { get; set; } = string.Empty;


		[Required]
		public int? CountryId { get; set; }

        public List<Country>? Countries { get; set; }

        [Required]
		[StringLength(4, MinimumLength = 4)]
		public string VendorCode { get; set; } = string.Empty;

		public string? Logo { get; set; } = string.Empty;
	}
}
