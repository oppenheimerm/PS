
namespace PS.Datastore.EFCore.Helpers.Geospatial
{
    /// <summary>
    /// Specifies a Latitude / Longitude point.
    /// </summary>
    public record LatLng
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Guid StationIdentifier { get; set; }

        public LatLng()
        {
        }

        public LatLng(double lat, double lng, Guid id)
        {
            this.Latitude = lat;
            this.Longitude = lng;
            this.StationIdentifier = id;
        }
    }
}
