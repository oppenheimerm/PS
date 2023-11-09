
using PS.Core.Models;
using PS.Core.ViewModels;

namespace PS.UseCases.Interfaces
{
    public interface IAddEmployeeUseCase
    {
        /// <summary>
        /// Add an (Employee) <see cref="Person"/> to the datastore
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task<(Person employee, bool Success, string ErrorMessage)> ExecuteAsync(AddEmployeeVM employee);
    }
}
