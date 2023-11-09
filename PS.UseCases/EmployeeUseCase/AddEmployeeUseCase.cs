using PS.Core.Models;
using PS.Core.ViewModels;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.EmployeeUseCase
{
    public class AddEmployeeUseCase : IAddEmployeeUseCase
    {
        private readonly IEmployeeRepository EmployeeRepository;

        public AddEmployeeUseCase(IEmployeeRepository employeeRepository)
        {
            EmployeeRepository = employeeRepository;
        }
        public async Task<(Person employee, bool Success, string ErrorMessage)> ExecuteAsync(AddEmployeeVM employee)
        {
            var response = await EmployeeRepository.Add(employee);
            return response;
        }


    }
}
