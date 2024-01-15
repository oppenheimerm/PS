
namespace PA.UseCases.Interfaces
{
    public interface IGetCountryCodeByIdUseCase
    {
        Task<string> ExecuteAsync(int countryId);
    }
}
