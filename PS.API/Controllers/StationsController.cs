using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PS.Core.Models.ApiRequestResponse;
using PS.UseCases.Interfaces;

namespace PS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [PS.API.Authorization.Attributes.Authorize]
    public class StationsController : ControllerBase
    {
        //public IList<Station> Stations { get; set; } = default!;
        public readonly IGetAllPetrolStationsFlatUseCase GetAllPetrolStationsFlatUseCase;

        public StationsController(IGetAllPetrolStationsFlatUseCase getAllPetrolStationsFlatUseCase)
        {
            GetAllPetrolStationsFlatUseCase = getAllPetrolStationsFlatUseCase;
        }

        // GET: api/Stations
        [HttpGet]
        public List<StationLite> GetStations()
        {
            return GetAllPetrolStationsFlatUseCase.Execute().ToList();
        }
    }
}
