
using Microsoft.AspNetCore.Mvc;
using PS.Core.Helpers;
using PS.Core.Helpers.Paging;
using PS.Core.Models.ApiRequestResponse;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.PetrolStationUseCase
{
    public class GetAllStationNearLatLongPoint : IGetAllStationNearLatLongPoint
    {
        private readonly IPetrolStationRepository PetrolStationRepository;

        public GetAllStationNearLatLongPoint(IPetrolStationRepository petrolStationRepository)
        {
            PetrolStationRepository = petrolStationRepository;
        }

        public PagedList<StationLite> Execute(double fromLat, double fromLongt, int countryId,
            DistanceUnit units, [FromQuery] PagingParameters pagingParms)
        {
            return PetrolStationRepository.GetAllStationsNearLocation(fromLat, fromLongt, countryId, units, pagingParms);
        }
    }
}
