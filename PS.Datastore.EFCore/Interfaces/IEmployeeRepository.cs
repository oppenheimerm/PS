
using PS.Core.Models;
using PS.Core.ViewModels;

namespace PS.Datastore.EFCore.Interfaces
{
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Get all Employees, private data is not returned with this query
        /// </summary>
        /// <returns></returns>
        IQueryable<EmployeeVM> GetEmployeeDirectory();
        /// <summary>
        /// Add a Employee <see cref="Person"/> to the datastore
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task<(Person employee, bool Success, string ErrorMessage)> Add(AddEmployeeVM employee);
        /// <summary>
        /// Add an <see cref="Employee"/> instance to the datastore.  This method works with the <see cref="Add(AddEmployeeVM)" />
        /// For adding an <see cref="Person"/> (IdentityUser to the datastore).
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task<(Employee Employee, bool Success, string ErrorMessage)> AddEmployeeObject(Employee employee);
    }
}
