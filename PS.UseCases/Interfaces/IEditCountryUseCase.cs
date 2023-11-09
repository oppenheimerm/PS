
using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IEditCountryUseCase
    {
        Task<(Country, bool success, string ErrorMessage)> ExecuteAsync(Country country);
    }
}
