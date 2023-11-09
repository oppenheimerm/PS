
using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IGetCountryCodeByCodeUseCase
    {
        Task<Country?> ExecuteAsync(string countryCode);
    }
}
