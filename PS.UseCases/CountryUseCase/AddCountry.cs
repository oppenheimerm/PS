using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.CountryUseCase
{
    public class AddCountry : IAddCountryUseCase
    {
        private readonly ICountryRepository CountryRepository;


        public AddCountry(ICountryRepository countryRepository)
        {
            CountryRepository = countryRepository;
        }

        public async Task<(Country Country, bool success, string ErrorMessage)> ExecuteAsync(Country country)
        {
            var response = await CountryRepository.Add(country);
            return response;
        }
    }
}
