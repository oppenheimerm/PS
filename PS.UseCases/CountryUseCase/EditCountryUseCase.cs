
using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.CountryUseCase
{
    public class EditCountryUseCase : IEditCountryUseCase
    {
        private readonly ICountryRepository CountryRepository;

        public EditCountryUseCase(ICountryRepository countryRepository)
        {
            CountryRepository = countryRepository;
        }

        public async Task<(Country, bool success, string ErrorMessage)> ExecuteAsync(Country country)
        {
            var response = await CountryRepository.Edit(country);
            return response;
        }
    }
}
