﻿
using System.Text.Json.Serialization;

namespace PA.Core.Models.ApiRequestResponse
{
	/// <summary>
	/// Flat DTO of <see cref="Core.Models.Station"/>
	/// </summary>
	public class StationLite
	{
		public int Id { get; set; }

		public string StationName { get; set; } = string.Empty;

		public string StationAddress { get; set; } = string.Empty;


		public string StationPostcode { get; set; } = string.Empty;

		public double? Latitude { get; set; }

		public double? Longitude { get; set; }

		public bool StationOnline { get; set; } = false;

		public string VendorName { get; set; } = string.Empty;
		public string Country { get; set; } = string.Empty;
		public bool PayByApp { get; set; } = false;
		public bool PayAtPump { get; set; } = false;
		public List<string>? Logos { get; set; }
		[JsonIgnore]
		public string Logo { get; set; } = string.Empty;
		public double? Distance { get; set; }
		public bool AccessibleToiletNearby { get; set; } = false;

	}
}
