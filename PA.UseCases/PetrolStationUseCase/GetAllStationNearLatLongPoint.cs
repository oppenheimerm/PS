
using Microsoft.AspNetCore.Mvc;
using PA.Core.Helpers;
using PA.Core.Helpers.Paging;
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.PetrolStationUseCase
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
