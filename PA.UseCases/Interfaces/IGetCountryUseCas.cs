
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IGetCountryUseCase
    {
        Task<Country?> ExecuteAsync(int id);
    }
}
