using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IGetAllCountriesUseCase
    {
        IQueryable<Country> Execute();
    }
}
