using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PS.Core.Helpers;
using PS.Core.ViewModels;
using PS.UseCases.Interfaces;
using PS.Web.Admin.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace PS.Web.Admin.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly IAddEmployeeUseCase AddEmployeeUseCase;
        private readonly IAddEmployeeObjectUseCase AddEmployeeObectUseCase;

        /*
[BindProperty]
        public AddPetrolStationVM AddPetrolStationVM { get; set; } = default!;
         */

        [BindProperty]
        public AddEmployeeVM AddEmployeeVM { get; set; } = default;


        public CreateModel(IAddEmployeeUseCase addEmployeeUseCase, IAddEmployeeObjectUseCase addEmployeeObectUseCase)
        {
            AddEmployeeUseCase = addEmployeeUseCase;
            AddEmployeeObectUseCase = addEmployeeObectUseCase;

        }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || AddEmployeeVM == null)
            {
                return Page();
            }
            else
            {
                //var title = AddEmployeeVM.Title;
                //var department = AddEmployeeVM.PrimaryDepartment;
                var titleInt = int.Parse(AddEmployeeVM.Title);
                AddEmployeeVM.Title = ((UserPrefix)titleInt).ToString();
                AddEmployeeVM.Email = AddEmployeeVM.UserName.ToLowerInvariant() + PS.Core.Models.Constants.EmailPostfix;
                

                var addStautus = await AddEmployeeUseCase.ExecuteAsync(AddEmployeeVM);
                if (addStautus.Success)
                {
                    // Add Employee to database
                    var addEmployeeObj = AddEmployeeVM.ToEmployee();
                    addEmployeeObj.Id = Guid.Parse(addStautus.employee.Id);
                    var addEmployeeObjStatus = await AddEmployeeObectUseCase.ExecuteAsync(addEmployeeObj);
                    if (addEmployeeObjStatus.Success)
                    {
                        return RedirectToPage("./Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, addEmployeeObjStatus.ErrorMessage);
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, addStautus.ErrorMessage);
                    return Page();
                }
            }
        }
    }
}
