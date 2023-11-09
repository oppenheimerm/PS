
using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IAddEmployeeObjectUseCase
    {
        Task<(Employee Employee, bool Success, string ErrorMessage)> ExecuteAsync(Employee employee);
    }
}
