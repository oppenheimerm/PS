
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PS.Core.Models;
using PS.Core.Models.ApiRequestResponse;
using PS.Datastore.EFCore.Interfaces;
using System.Numerics;

namespace PS.Datastore.EFCore.Repositories
{
    public class PetrolStationRepository : IPetrolStationRepository
    {
        private readonly ILogger<PetrolStationRepository> Logger;
        private readonly ApplicationDbContext Context;

        public PetrolStationRepository(ILogger<PetrolStationRepository> logger, ApplicationDbContext context)
        {
            Logger = logger;
            Context = context;
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
                            Country = country.CountryName
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
