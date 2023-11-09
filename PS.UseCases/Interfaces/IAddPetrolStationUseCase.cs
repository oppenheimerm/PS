
using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IAddPetrolStationUseCase
    {
        Task<(Station Station, bool success, string ErrorMessage)> ExecuteAsync(Station station);
    }
}
