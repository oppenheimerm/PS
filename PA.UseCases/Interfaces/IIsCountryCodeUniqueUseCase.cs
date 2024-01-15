
namespace PA.UseCases.Interfaces
{
    public interface IIsCountryCodeUniqueUseCase
    {
        Task<bool> ExecuteAsync(string CountryCode);
    }
}
