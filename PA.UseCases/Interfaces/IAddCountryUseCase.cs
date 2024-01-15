
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IAddCountryUseCase
    {
        Task<(Country Country, bool success, string ErrorMessage)> ExecuteAsync(Country country);
    }
}
