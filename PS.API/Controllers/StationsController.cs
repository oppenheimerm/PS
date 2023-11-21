using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PS.Core.Helpers;
using PS.Core.Models.ApiRequestResponse;
using PS.UseCases.Interfaces;

namespace PS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [PS.API.Authorization.Attributes.Authorize]
    public class StationsController : BaseController
    {
        //public IList<Station> Stations { get; set; } = default!;
        public readonly IGetAllPetrolStationsFlatUseCase GetAllPetrolStationsFlatUseCase;
        private readonly IWebHostEnvironment WebHostingEnvironment;

        public StationsController(IGetAllPetrolStationsFlatUseCase getAllPetrolStationsFlatUseCase, IWebHostEnvironment webHostingEnvironment)
        {
            GetAllPetrolStationsFlatUseCase = getAllPetrolStationsFlatUseCase;
            WebHostingEnvironment = webHostingEnvironment;
        }

        // GET: api/Stations
        [HttpGet]
        public List<StationLite> GetStations()
        {
            var query = GetAllPetrolStationsFlatUseCase.Execute().ToList();
            var stations = GetStationLogos(query);
            return stations;
        }

        private List<string> GetLogosForStation( string logo)
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

        private List<StationLite> GetStationLogos(List<StationLite> stations)
        {
            foreach (StationLite station in stations)
            {
                station.Logos = GetLogosForStation(station.Logo);
            }

            return stations;
        }
    }
}
