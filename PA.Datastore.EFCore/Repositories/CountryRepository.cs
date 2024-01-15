
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;

namespace PA.Datastore.EFCore.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ILogger<CountryRepository> Logger;
        private readonly ApplicationDbContext Context;

        public CountryRepository(ILogger<CountryRepository> logger, ApplicationDbContext contenxt)
        {
            Logger = logger;
            Context = contenxt;
        }

        public IQueryable<Country> GetAll()
        {
            var countries = Context.Countries.AsNoTracking();
            return countries;
        }

        public async Task<(Country country, bool Success, string ErrorMessage)> Add(Country country)
        {
            // unique vendor code?
            var isUnique = await IsCountryCodeUnique(country.CountryCode);
            if (isUnique)
            {
                try
                {
                    country.CountryCode = country.CountryCode?.ToUpperInvariant();
                    Context.Countries.Add(country);
                    await Context.SaveChangesAsync();
                    Logger.LogInformation($"Country with Id: {country.Id}, added to database at: {DateTime.UtcNow}");
                    return (country, true, string.Empty);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to add country to database. Timestamp : {DateTime.UtcNow}");
                    return (country, false, ex.ToString());
                }
            }
            else
            {
                var errMsg = $"Failed to add country with country code {country.CountryCode} to database.  Code already in use. Timestamp : {DateTime.UtcNow}";
                Logger.LogError(errMsg);
                return (country, false, errMsg);
            }
        }

        public async Task<(Country Country, bool Success, string ErrorMessage)> Edit(Country country)
        {
            // unique vendor code?
            var isUnique = await IsCountryCodeUnique(country?.CountryCode);
            if (isUnique)
            {
                try
                {
                    country.CountryCode = country.CountryCode?.ToUpperInvariant();
                    Context.Countries.Update(country);
                    await Context.SaveChangesAsync();
                    return (country, true, string.Empty);
                }
                catch (DbUpdateException /* ex */)
                {
                    Logger.LogError($"Failed to update an instance of Country at: {DateTime.UtcNow}");
                    return (new Country(), false, $"Failed to update an instance of Country at: {DateTime.UtcNow}");
                }
            }
            else
            {
                var errMsg = $"Failed to add country with country code {country.CountryCode} to database.  Code already in use. Timestamp : {DateTime.UtcNow}";
                Logger.LogError(errMsg);
                return (country, false, errMsg);
            }
        }

        public async Task<Country?> GetCountry(int id)
        {
            return await Context.Countries.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<string?> GetCountryCodeFromId(int id)
        {
            var countryCode = await Context.Countries.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            return countryCode == null ? null : countryCode.CountryCode;
        }

        public async Task<Country?> GetCountryFromCountryCode(string countryCode)
        {

            return await Context.Countries.AsNoTracking().FirstOrDefaultAsync(c => c.CountryCode == countryCode.ToUpperInvariant());
        }


        // Helpers
        public async Task<bool> IsCountryCodeUnique(string countryCode)
        {
            var codeInUse = await Context.Countries
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CountryCode == countryCode.ToUpper());

            return (codeInUse == null) ? true : false;
        }
    }
}
