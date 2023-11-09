using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.EmployeeUseCase
{
    public class AddEmployeeObjectUseCase : IAddEmployeeObjectUseCase
    {
        private IEmployeeRepository EmployeeRepository;

        public AddEmployeeObjectUseCase(IEmployeeRepository employeeRepository)
        {
            EmployeeRepository = employeeRepository;
        }

        public async Task<(Employee Employee, bool Success, string ErrorMessage)> ExecuteAsync(Employee employee)
        {
            var response = await EmployeeRepository.AddEmployeeObject(employee);
            return response;
        }
    }
}
