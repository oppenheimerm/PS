
using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IGetPetrolStationByIdUseCase
    {
        Task<Station?> ExecuteAsync(int id);
    }
}
