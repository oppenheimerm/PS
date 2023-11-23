using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PS.Core.Helpers;
using PS.Core.Helpers.Paging;
using PS.Core.Models.ApiRequestResponse;
using PS.UseCases.Interfaces;

namespace PS.API.Controllers.V1
{
    /// <summary>
    /// Manages all request for Station data.  User must be authenticated
    /// </summary>
    [Authorization.Attributes.Authorize]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/stations")]
    [ApiController]
    public class StationsController : BaseController
    {
        //public IList<Station> Stations { get; set; } = default!;
        public readonly IGetAllPetrolStationsFlatUseCase GetAllPetrolStationsFlatUseCase;
        private readonly IWebHostEnvironment WebHostingEnvironment;
        private readonly IGetAllStationNearLatLongPoint GetAllStationNearLatLongPoint;

        public StationsController(IGetAllPetrolStationsFlatUseCase getAllPetrolStationsFlatUseCase, IWebHostEnvironment webHostingEnvironment,
            IGetAllStationNearLatLongPoint iGetAllStationNearLatLongPoint)
        {
            GetAllPetrolStationsFlatUseCase = getAllPetrolStationsFlatUseCase;
            WebHostingEnvironment = webHostingEnvironment;
            GetAllStationNearLatLongPoint = iGetAllStationNearLatLongPoint;
        }

        [HttpGet("nearby-stations")]
        public GetNearestStationsResponse NearbyStations(double fromLat, double fromLongt, int countryId,
            DistanceUnit units, [FromQuery] PagingParameters pagingParms)
        {

            var response = new GetNearestStationsResponse();
            var query = GetAllStationNearLatLongPoint.Execute(fromLat, fromLongt, countryId, units, pagingParms);

            response.Stations = GetStationLogos(query);
            response.TotalCount = query.TotalCount;
            response.PageSize = query.PageSize;
            response.CurrentPage = query.CurrentPage;
            response.TotalPages = query.TotalPages;
            response.HasNext = query.HasNext;
            response.HasPrevious = query.HasPrevious;


            return response;

        }

        private List<string> GetLogosForStation(string logo)
        {
            //"C:\\Users\\moppenheimer\\repo\\web\\PS\\PS.API\\wwwroot\\img\\logosasda_logo_80_x_80.jpg",
            //var path2 = $"/img/logos/{}";

            List<string> Logos = new List<string>();
            //var basePath = Path.Combine(WebHostingEnvironment.WebRootPath, "img", "logos");

            var ExtraSmall = $"{VendorLogoHelper.GetVendorLogo(logo, VendorLogoSize.ExtraSmall)}";
            Logos.Add(ExtraSmall);

            var Small = $"{VendorLogoHelper.GetVendorLogo(logo, VendorLogoSize.Small)}";
            Logos.Add(Small);

            var Medium = $"{VendorLogoHelper.GetVendorLogo(logo, VendorLogoSize.Medium)}";
            Logos.Add(Medium);

            var Large = $"{VendorLogoHelper.GetVendorLogo(logo, VendorLogoSize.Large)}";
            return Logos;
        }

        /// <summary>
        /// Helper function, to get the respective <see cref="Vendor"/> logo for 
        /// <see cref="StationLite"/> instance.
        /// </summary>
        /// <param name="stations"></param>
        /// <returns></returns>
        private PagedList<StationLite> GetStationLogos(PagedList<StationLite> stations)
        {
            foreach (StationLite station in stations)
            {
                station.Logos = GetLogosForStation(station.Logo);
            }

            return stations;
        }
    }
}
