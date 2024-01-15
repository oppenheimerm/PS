
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IAddPetrolStationUseCase
    {
        Task<(Station Station, bool success, string ErrorMessage)> ExecuteAsync(Station station);
    }
}
