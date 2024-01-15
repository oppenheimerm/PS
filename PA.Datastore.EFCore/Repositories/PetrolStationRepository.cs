
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PA.Core.Helpers;
using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Helpers.Geospatial;
using PA.Datastore.EFCore.Interfaces;

namespace PA.Datastore.EFCore.Repositories
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
            // PostCode exist
            var postCodeInUse = await PostCodeInUseAsync(station.StationPostcode);
            if (postCodeInUse)
            {
                var errorMsg = $"Attempted to add station with duplicate postcode: {station.StationPostcode}. Timestamp: {DateTime.UtcNow}";
                Logger.LogError(errorMsg);
                return (new Station(), false, errorMsg);
            }
            try
            {
                station.StationPostcode = station.StationPostcode.ToUpperInvariant();
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

        public async Task<(Station station, bool success, string ErrorMessage)> Edit(Station station)
        {
            try
            {
                station.StationPostcode = station.StationPostcode.ToUpperInvariant();
                Context.PetrolStations.Update(station);
                await Context.SaveChangesAsync();
                Logger.LogInformation($"Station with Id: {station.Id}, and name of: {station.StationName} was updated at: {DateTime.UtcNow}");
                return (station, true, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to update station: {station.StationName}. Timestamp : {DateTime.UtcNow}");
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

        // Paged nearest stations
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
                            StationPostcode = station.StationPostcode,
                            Latitude = station.Latitude,
                            Longitude = station.Longitude,
                            StationOnline = station.StationOnline,
                            VendorName = vendor.VendorName,
                            Country = country.CountryName,
                            Logo = vendor.Logo,
                            PayAtPump = station.PayAtPump,
                            PayByApp = station.PayByApp,
                            AccessibleToiletNearby = station.AccessibleToiletNearby
                        };

            //  Execution of the query is deferred until the query variable is iterated over in a foreach,
            //  For Each loop or ToList(). 



            /*return PagedList<Owner>.ToPagedList(FindAll().OrderBy(on => on.Name),
                ownerParameters.PageNumber,
                ownerParameters.PageSize);*/

            var stations = PagedList<StationLite>.ToPagedList(query, pagingParms.PageNumber, pagingParms.PageSize);
            for (int i = 0; i < stations.Count; i++)
            {

                stations[i].Distance = Math.Round(GeoHelpers.HaversineDistance(fromLat, fromLongt, stations[i], units), 2);
            }
            if (stations != null)
            {
                Logger.LogInformation($" Returned {stations.Count} items for query near this geo location at: {DateTime.UtcNow}");
            }
            else
            {
                Logger.LogInformation($" Could not find any locations near this geolocation. TimeStampe: {DateTime.UtcNow}");
            }
            return stations;
        }


        public List<StationLite> GetStationsNearUser(double fromLat, double fromLongt, int countryId,
           DistanceUnit units)
        {
            var latAsFloat = float.Parse(fromLat.ToString());
            var longAsFloat = float.Parse(fromLongt.ToString());


            var table = from station in Context.PetrolStations
                        join country in Context.Countries on station.CountryId equals country.Id
                        join vendor in Context.PetrolVendors on station.VendorId equals vendor.Id
                        where country.Id == countryId
                        select new StationLite
                        {
                            Id = station.Id,
                            StationName = station.StationName,
                            StationAddress = station.StationAddress,
                            StationPostcode = station.StationPostcode,
                            Latitude = station.Latitude,
                            Longitude = station.Longitude,
                            StationOnline = station.StationOnline,
                            VendorName = vendor.VendorName,
                            Country = country.CountryName,
                            Logo = vendor.Logo,
                            PayAtPump = station.PayAtPump,
                            PayByApp = station.PayByApp,
                            AccessibleToiletNearby = station.AccessibleToiletNearby,
                            Distance = Context.HaversineDistance(latAsFloat, longAsFloat,
                            (float)station.Latitude.Value,
                            (float)station.Longitude.Value,
                            (int)units)
                        };


            var stations = table
                .OrderBy(s => s.Distance)
                .Take(20)
                .ToList();

            Logger.LogInformation($"Found {stations.Count} near user.");

            return stations;
        }

        public IQueryable<StationLite> GetAllFlat()
        {
            var query = from station in Context.PetrolStations
                        join country in Context.Countries on station.CountryId equals country.Id
                        join vendor in Context.PetrolVendors on station.VendorId equals vendor.Id
                        select new StationLite
                        {
                            Id = station.Id,
                            StationName = station.StationName,
                            StationAddress = station.StationAddress,
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

        public async Task<Station?> GetStationById(int id)
        {
            return await Context.PetrolStations
                .Include(v => v.Vendor)
                .AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> PostCodeInUseAsync(string postCode)
        {
            // PostCodes are stored in uppercase.
            var postCodeToCompare = postCode.ToUpperInvariant();

            var exist = await Context.PetrolStations
                .Where(s => s.StationPostcode == postCodeToCompare)
                .AsNoTracking().FirstOrDefaultAsync();


            if (exist != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
