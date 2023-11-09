using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.Core.ViewModels;
using PS.UseCases.Interfaces;

namespace PS.Web.Admin.Pages.Employees
{
    public class IndexModel : PageModel
    {
        //public IList<Station> Stations { get; set; } = default!;
        public IList<EmployeeVM> Employees { get; set; } = default(IList<EmployeeVM>);
        private readonly IGetEmployeesDirectoryUseCase GetEmployeesDirectoryUseCase;

        public IndexModel(IGetEmployeesDirectoryUseCase getAllEmployeesDirectoryUseCase)
        {
            GetEmployeesDirectoryUseCase = getAllEmployeesDirectoryUseCase;
        }
        public void OnGet()
        {
            Employees = GetEmployeesDirectoryUseCase.Execute().ToList();
        }
    }
}
