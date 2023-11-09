
using PS.Core.ViewModels;

namespace PS.UseCases.Interfaces
{
    public interface IGetEmployeesDirectoryUseCase
    {
        IQueryable<EmployeeVM> Execute();
    }
}
