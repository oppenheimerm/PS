
using Microsoft.AspNetCore.Mvc;
using PS.Core.Helpers;
using PS.Core.Helpers.Paging;
using PS.Core.Models.ApiRequestResponse;

namespace PS.UseCases.Interfaces
{
    public interface IGetAllStationNearLatLongPoint
    {
        PagedList<StationLite> Execute(double fromLat, double fromLongt, int countryId,
                    DistanceUnit units, [FromQuery] PagingParameters pagingParms);
    }
}
