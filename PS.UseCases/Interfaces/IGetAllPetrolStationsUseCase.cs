
using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IGetAllPetrolStationsUseCase
    {
        IQueryable<Station> Execute();
    }
}
