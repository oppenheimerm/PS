using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.CountryUseCase
{
    public class GetCountryUseCase : IGetCountryUseCase
    {
        private readonly ICountryRepository CountryRepository;

        public GetCountryUseCase(ICountryRepository countryRepository)
        {
            CountryRepository = countryRepository;
        }

        public async Task<Country?> ExecuteAsync(int id)
        {
            return await CountryRepository.GetCountry(id);
        }
    }
}
