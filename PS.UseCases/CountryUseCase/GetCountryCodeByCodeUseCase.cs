
using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.CountryUseCase
{
    public class GetCountryCodeByCodeUseCase : IGetCountryCodeByCodeUseCase
    {
        public readonly ICountryRepository CountryRepository;

        public GetCountryCodeByCodeUseCase(ICountryRepository countryRepository)
        {
            CountryRepository = countryRepository;  
        }

        public async Task<Country?> ExecuteAsync(string countryCode)
        {
            var response = await CountryRepository.GetCountryFromCountryCode(countryCode);
            return response;
        }
    }
}
