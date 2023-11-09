
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PS.Core.Models;
using PS.Core.ViewModels;
using PS.Datastore.EFCore.Interfaces;
using System.Numerics;
using static System.Collections.Specialized.BitVector32;

namespace PS.Datastore.EFCore.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly UserManager<Person> UserManager;
        private readonly ILogger<EmployeeRepository> Logger;
        private readonly ApplicationDbContext Context;
        //private readonly RoleManager<Person> RoleManager;

        public EmployeeRepository(
            UserManager<Person> userManager, 
            ILogger<EmployeeRepository> logger,
            ApplicationDbContext context)
        {
            UserManager = userManager; 
            Logger = logger;
            Context = context;
        }


        public IQueryable<EmployeeVM> GetEmployeeDirectory()
        {
            // https://www.c-sharpcorner.com/article/list-of-users-with-roles-in-mvc-asp-net-identity/
            // https://gavilan.blog/2020/05/15/getting-users-and-their-role-in-asp-net-core-aspnetusers-and-aspnetuserroles/
            var query = from employee in Context.Employees
                        //join userRoles in RoleManager on user.Id equals userRole.Id
                        //join roles in RoleManager on user.ro
                        //join userRole in RoleManager.Roles on user.Id equals userRole.Id into roles
                        select new EmployeeVM
                        {
                            Id = employee.Id,
                            JoinDate = employee.JoinDate,
                            FirstName = employee.FirstName,
                            LasttName = employee.LasttName,
                            Initials = employee.Initials,
                            PrimaryDepartment = employee.PrimaryDepartment,
                            Title = employee.Title,
                            Photo = employee.Photo
                        };
            Logger.LogInformation($"Retrieved Employees from the database at: {DateTime.UtcNow}");

            return query;
        }

        //public async Task<(Vendor vendor, bool Success, string ErrorMessage)> Add(Vendor vendor)#
        public async Task<(Person employee, bool Success, string ErrorMessage)> Add(AddEmployeeVM employee)
        {
            //  var user = await userManager.FindByNameAsync(initUser);
            var user = await UserManager.FindByEmailAsync(employee.Email);
            if (user == null)
            {

                Person Employee = new Person()
                {
                    UserName = employee.UserName.ToLowerInvariant(),
                    Email = employee.Email,
                    DOB = employee.DOB
                };

                Employee.EmailConfirmed = true;

                IdentityResult result = await UserManager.CreateAsync(Employee, employee.Password);
                if (result.Succeeded)
                {
                    Logger.LogInformation($"Employee with Id: {Employee.Id} and Username: {Employee.UserName} add at {DateTime.UtcNow}");
                    return (Employee, true, string.Empty);
                }
                else
                {
                    Logger.LogError($"Failed to add {employee.UserName} to databaseEmployee with Id: {Employee.Id} and Username: {Employee.UserName} add at {DateTime.UtcNow}");
                    var errorList = result.Errors.ToList();
                    var errors = string.Join(", ", errorList.Select(e => e.Description));
                    return (Employee, false, errors);
                }

            }
            else
            {
                var errMsg = $"Failed to add {employee.UserName} to database, the username: {employee.UserName} is already in use. {DateTime.UtcNow}";
                Logger.LogError(errMsg);
                return (new Person(), false, errMsg);
            }
        }

        public async Task<(Employee Employee, bool Success, string ErrorMessage)> AddEmployeeObject(Employee employee)
        {
            try { 
                Context.Employees.Add(employee);
                await Context.SaveChangesAsync();
                Logger.LogInformation($"Employee with Id: {employee.Id}, and username of: {employee.UserName} added to database at: {DateTime.UtcNow}");
                return (employee, true, string.Empty);
            }
            catch(Exception ex)
            {
                Logger.LogError($"Failed to add employee to database. Timestamp : {DateTime.UtcNow}");
                return(new Employee(), false, ex.ToString());
            }
        }
    }
}
