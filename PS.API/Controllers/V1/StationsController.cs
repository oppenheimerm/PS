using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PS.Core.Helpers;
using PS.Core.Helpers.Paging;
using PS.Core.Models.ApiRequestResponse;
using PS.UseCases.Interfaces;

namespace PS.API.Controllers.V1
{
    /// <summary>
    /// Manages all request for Station data.  User must be authenticated
    /// </summary>
    [Route("api/[controller]")]
	[ApiVersion("1.0")]
	[ApiController]
    [Authorization.Attributes.Authorize]
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

        /// <summary>
        /// Get all Stations <see cref="StationLite"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        /*public List<StationLite> GetStations()
        {
            var query = GetAllPetrolStationsFlatUseCase.Execute().ToList();
            var stations = GetStationLogos(query);
            return stations;
        }*/

        [HttpGet("statios-nearby")]
        public List<StationLite>GetNearbayStation(double fromLat, double fromLongt, int countryId,
            DistanceUnit units, [FromQuery] PagingParameters pagingParms)
        {
            var query = GetAllStationNearLatLongPoint.Execute(fromLat, fromLongt, countryId, units, pagingParms);
            var stations = GetStationLogos(query);

            var metadata = new
            {
                stations.TotalCount,
                stations.PageSize,
                stations.CurrentPage,
                stations.TotalPages,
                stations.HasNext,
                stations.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return stations.OrderBy( s => s.Distance).ToList();

        }

        private List<string> GetLogosForStation(string logo)
        {
            //"C:\\Users\\moppenheimer\\repo\\web\\PS\\PS.API\\wwwroot\\img\\logosasda_logo_80_x_80.jpg",
            //var path2 = $"/img/logos/{}";

            List<string> Logos = new List<string>();
            //var basePath = Path.Combine(WebHostingEnvironment.WebRootPath, "img", "logos");

            var ExtraSmall = $"/img/logos/{VendorLogoHelper.GetVendorLogo(logo, VendorLogoSize.ExtraSmall)}";
            Logos.Add(ExtraSmall);

            var Small = $"/img/logos/{VendorLogoHelper.GetVendorLogo(logo, VendorLogoSize.Small)}";
            Logos.Add(Small);

            var Medium = $"/img/logos/{VendorLogoHelper.GetVendorLogo(logo, VendorLogoSize.Medium)}";
            Logos.Add(Medium);

            var Large = $"/img/logos/{VendorLogoHelper.GetVendorLogo(logo, VendorLogoSize.Large)}";
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
