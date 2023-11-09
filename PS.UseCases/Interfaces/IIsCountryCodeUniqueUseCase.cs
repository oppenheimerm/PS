
namespace PS.UseCases.Interfaces
{
	public interface IIsCountryCodeUniqueUseCase
	{
		Task<bool> ExecuteAsync(string CountryCode);
	}
}
