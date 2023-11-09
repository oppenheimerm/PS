using PS.Core.Models;

namespace PS.Datastore.EFCore.Interfaces
{
    public interface ICountryRepository
    {
        /// <summary>
        /// Get and instance of a <see cref="Country"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Country?> GetCountry(int id);
        /// <summary>
        /// Get all <see cref="Country"/> as IQuerable list.
        /// </summary>
        /// <returns></returns>
        IQueryable<Country> GetAll();
        Task<(Country country, bool Success, string ErrorMessage)> Add(Country country);
        Task<(Country Country, bool Success, string ErrorMessage)> Edit(Country country);
		Task<bool> IsCountryCodeUnique(string countryCode);
        /// <summary>
        /// Helper method for retrieving a <see cref="Country"/> by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string?> GetCountryCodeFromId(int id);
        /// <summary>
        /// Get <see cref="Country"/> from Country.CountryCode (string)
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        Task<Country?> GetCountryFromCountryCode(string countryCode);

    }
}
