
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IEditCountryUseCase
    {
        Task<(Country, bool success, string ErrorMessage)> ExecuteAsync(Country country);
    }
}
