using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IAddCountryUseCase
    {
        Task<(Country Country, bool success, string ErrorMessage)> ExecuteAsync(Country country);
    }
}
