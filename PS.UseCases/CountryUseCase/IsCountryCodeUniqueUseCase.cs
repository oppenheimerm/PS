using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.CountryUseCase
{
	public class IsCountryCodeUniqueUseCase : IIsCountryCodeUniqueUseCase
	{
		private readonly ICountryRepository CountryRepository;

        public IsCountryCodeUniqueUseCase(ICountryRepository countryRepository)
        {
            CountryRepository = countryRepository;
        }
		public async Task<bool> ExecuteAsync(string CountryCode)
		{
			var response = await CountryRepository.IsCountryCodeUnique(CountryCode);
			return response;
		}
	}
}
