
using PA.Core.ViewModels;

namespace PA.UseCases.Interfaces
{
    public interface IGetEmployeesDirectoryUseCase
    {
        IQueryable<EmployeeVM> Execute();
    }
}
