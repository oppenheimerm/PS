
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IGetPetrolStationByIdUseCase
    {
        Task<Station?> ExecuteAsync(int id);
    }
}
