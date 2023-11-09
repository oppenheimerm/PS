using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.CountryUseCase
{
    public class GetAllCountriesUseCase : IGetAllCountriesUseCase
    {
        private readonly ICountryRepository CountryRepository;

        public GetAllCountriesUseCase(ICountryRepository countryRepository)
        {
            CountryRepository = countryRepository;
        }

        public IQueryable<Country> Execute()
        {
            return CountryRepository.GetAll();
        }
    }
}
