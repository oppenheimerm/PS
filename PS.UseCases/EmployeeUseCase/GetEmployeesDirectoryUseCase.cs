
using PS.Core.ViewModels;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.EmployeeUseCase
{
    public class GetEmployeesDirectoryUseCase : IGetEmployeesDirectoryUseCase
    {
        private readonly IEmployeeRepository EmployeeRepository;

        public GetEmployeesDirectoryUseCase(IEmployeeRepository employeeRepository)
        {
            EmployeeRepository = employeeRepository;
        }
        public IQueryable<EmployeeVM> Execute()
        {
            return EmployeeRepository.GetEmployeeDirectory();
        }
    }
}
