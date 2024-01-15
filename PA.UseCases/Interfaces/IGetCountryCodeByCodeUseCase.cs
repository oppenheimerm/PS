
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IGetCountryCodeByCodeUseCase
    {
        Task<Country?> ExecuteAsync(string countryCode);
    }
}
