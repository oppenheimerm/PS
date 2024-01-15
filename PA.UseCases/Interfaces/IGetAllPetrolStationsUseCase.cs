
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IGetAllPetrolStationsUseCase
    {
        IQueryable<Station> Execute();
    }
}
