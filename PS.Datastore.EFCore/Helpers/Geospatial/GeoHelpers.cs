using PS.Core.Helpers;
using PS.Core.Models.ApiRequestResponse;

namespace PS.Datastore.EFCore.Helpers.Geospatial
{
    //https://stormconsultancy.co.uk/blog/storm-news/the-haversine-formula-in-c-and-sql/
    public static class GeoHelpers
    {        

        /// <summary>
        /// Returns the distance in miles or kilometers of any two
        /// latitude / longitude points.
        /// </summary>
        /// <param name="pos1">Location 1</param>
        /// <param name="pos2">Location 2</param>
        /// <param name="unit">Miles or Kilometers</param>
        /// <returns>Distance in the requested unit</returns>
        public static double HaversineDistance(double fromLat, double fromLongt, StationLite latLongPost2, DistanceUnit unit)
        {
            double R = (unit == DistanceUnit.Miles) ? 3960 : 6371;
            var lat = ToRadians((latLongPost2.Latitude.Value - fromLat));
            var lng = ToRadians((latLongPost2.Longitude.Value - fromLongt));
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                          Math.Cos(ToRadians(fromLat)) * Math.Cos(ToRadians(latLongPost2.Latitude.Value)) *
                          Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return R * h2;
        }


        /// <summary>
        /// Convert to Radians.
        /// </summary>
        /// <param name="val">The value to convert to radians</param>
        /// <returns>The value in radians</returns>
        public static double ToRadians(double val)
        {
            return Math.PI / 180 * val;
        }
    }
}
