
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PS.Core.Helpers;
using PS.Core.Models;
using PS.Core.Models.ApiRequestResponse;
using PS.Datastore.EFCore.Helpers.Geospatial;
using PS.Datastore.EFCore.Interfaces;
using PS.Core.Helpers.Paging;
using static System.Collections.Specialized.BitVector32;

namespace PS.Datastore.EFCore.Repositories
{
    public class PetrolStationRepository : IPetrolStationRepository
    {
        private readonly ILogger<PetrolStationRepository> Logger;
        private readonly ApplicationDbContext Context;
        private readonly IWebHostEnvironment WebHostEnvironment;

        public PetrolStationRepository(ILogger<PetrolStationRepository> logger, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            Logger = logger;
            Context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        public async Task<(Station station, bool Success, string ErrorMessage)> Add(Station station)
        {
            try
            {
                Context.PetrolStations.Add(station);
                await Context.SaveChangesAsync();
                Logger.LogInformation($"Station with Id: {station.Id}, and name of: {station.StationName} added to database at: {DateTime.UtcNow}");
                return (station, true, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to add station to database. Timestamp : {DateTime.UtcNow}");
                return (station, false, ex.ToString());
            }
        }

        public IQueryable<Station> GetAll()
        {
            var stations = Context.PetrolStations
                .Include(v => v.Vendor)
                .AsNoTracking();
            return stations;
        }

        public PagedList<StationLite> GetAllStationsNearLocation(double fromLat, double fromLongt, int countryId, 
            DistanceUnit units, [FromQuery] PagingParameters pagingParms)
        {
            var query = from station in Context.PetrolStations
                        join country in Context.Countries on station.CountryId equals country.Id
                        join vendor in Context.PetrolVendors on station.VendorId equals vendor.Id
                        where country.Id == countryId
                        select new StationLite
                        {
                            Id = station.Id,
                            StationName = station.StationName,
                            StationAddress = station.StationAddress,
                            StationAddress2 = station.StationAddress2,
                            StationPostcode = station.StationPostcode,
                            Latitude = station.Latitude,
                            Longitude = station.Longitude,
                            StationOnline = station.StationOnline,
                            VendorName = vendor.VendorName,
                            Country = country.CountryName,
                            Logo = vendor.Logo
                        };

            //  Execution of the query is deferred until the query variable is iterated over in a foreach,
            //  For Each loop or ToList(). 



            /*return PagedList<Owner>.ToPagedList(FindAll().OrderBy(on => on.Name),
                ownerParameters.PageNumber,
                ownerParameters.PageSize);*/

            var stations = PagedList<StationLite>.ToPagedList(query, pagingParms.PageNumber, pagingParms.PageSize);
            for (int i = 0; i < stations.Count; i++)
            {

                stations[i].Distance = Math.Round( GeoHelpers.HaversineDistance(fromLat, fromLongt, stations[i], units), 2);
            }
            if(stations != null)
            {
                Logger.LogInformation($" Returned {stations.Count} items for query near this geo location at: {DateTime.UtcNow}");
            }
            else
            {
                Logger.LogInformation($" Could not find any locations near this geolocation. TimeStampe: {DateTime.UtcNow}");
            }
            return stations;
        }

        public IQueryable<StationLite>GetAllFlat()
        {
            var query = from station in Context.PetrolStations
                        join country in Context.Countries on station.CountryId equals country.Id
                        join vendor in Context.PetrolVendors on station.VendorId equals vendor.Id
                        select new StationLite
                        {
                            Id = station.Id,
                            StationName = station.StationName,
                            StationAddress = station.StationAddress,
                            StationAddress2 = station.StationAddress2,
                            StationPostcode = station.StationPostcode,
                            Latitude = station.Latitude,
                            Longitude = station.Longitude,
                            StationOnline = station.StationOnline,
                            VendorName = vendor.VendorName,
                            Country = country.CountryName,
                            Logo = vendor.Logo
                        };
            //
            query.OrderBy(s => s.StationName);
            query.Cast<object>().ToList();
            return query;
        }

        public async Task<Station?>GetStationById(int id)
        {
            return await Context.PetrolStations
                .Include(v => v.Vendor)
                .AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
