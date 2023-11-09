
using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IGetAllVendorsUseCase
    {
        IQueryable<Vendor> Execute();
    }
}
