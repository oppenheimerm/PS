using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.CountryUseCase
{   
    public class GetCountryCodeByIdUseCase : IGetCountryCodeByIdUseCase
    {
        public readonly ICountryRepository CountryRepository;

        public GetCountryCodeByIdUseCase(ICountryRepository countryRepository)
        {
            CountryRepository = countryRepository;
        }

        public async Task<string> ExecuteAsync(int countryId)
        {
            var response = await CountryRepository.GetCountryCodeFromId(countryId);
            return response;
        }
    }
}
