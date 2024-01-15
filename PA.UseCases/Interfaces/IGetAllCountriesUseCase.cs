
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IGetAllCountriesUseCase
    {
        IQueryable<Country> Execute();
    }
}
