using Microsoft.AspNetCore.Mvc;
using PA.Core.Helpers;
using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;

namespace PA.Datastore.EFCore.Interfaces
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
        /// Edit / Update a <see cref="Station"/> entry.
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        Task<(Station station, bool success, string ErrorMessage)> Edit(Station station);
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
        /// Get stations near user. Not paged.
        /// </summary>
        /// <param name="fromLat"></param>
        /// <param name="fromLongt"></param>
        /// <param name="countryId"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        List<StationLite> GetStationsNearUser(double fromLat, double fromLongt, int countryId,
                   DistanceUnit units);
        /// <summary>
        /// Get <see cref="Station"/> by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Station?> GetStationById(int id);
    }
}
