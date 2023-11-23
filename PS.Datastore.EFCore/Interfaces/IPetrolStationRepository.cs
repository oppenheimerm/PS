
using Microsoft.AspNetCore.Mvc;
using PS.Core.Helpers;
using PS.Core.Helpers.Paging;
using PS.Core.Models;
using PS.Core.Models.ApiRequestResponse;

namespace PS.Datastore.EFCore.Interfaces
{
    public interface IPetrolStationRepository
    {
        /// <summary>
        /// Add a <see cref="Station"/> to the database
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        Task<(Station station, bool Success, string ErrorMessage)> Add(Station station);
        /// <summary>
        /// Get all <see cref="Station"/> 's from database
        /// </summary>
        /// <returns></returns>
        IQueryable<Station> GetAll();
        /// <summary>
        /// Returns a subset(flat) list of <see cref="Station"/> Properties.  Used with the public API
        /// </summary>
        /// <returns></returns>
        IQueryable<StationLite> GetAllFlat();
        /// <summary>
        /// Returns a <see cref="PagedList{T}"/> of <see cref="StationLite"/> objects ordered by distance
        /// </summary>
        /// <param name="fromLat"></param>
        /// <param name="fromLongt"></param>
        /// <param name="countryId"></param>
        /// <param name="units"></param>
        /// <param name="pagingParms"></param>
        /// <returns></returns>
        PagedList<StationLite> GetAllStationsNearLocation(double fromLat, double fromLongt, int countryId,
                    DistanceUnit units, [FromQuery] PagingParameters pagingParms);
        /// <summary>
        /// Get <see cref="Station"/> by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Station?> GetStationById(int id);
    }
}
