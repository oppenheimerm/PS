using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IGetCountryUseCase
    {
        Task<Country?> ExecuteAsync(int id);
    }
}
