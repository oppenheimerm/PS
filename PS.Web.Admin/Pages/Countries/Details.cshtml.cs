using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.UseCases.Interfaces;

namespace PS.Web.Admin.Pages.Countries
{
    public class DetailsModel : PageModel
    {
        public readonly IGetCountryUseCase GetCountryUseCase;
        public PS.Core.Models.Country Country { get; set; } = default;
        public DetailsModel(IGetCountryUseCase getCountryUseCase)
        {
            GetCountryUseCase = getCountryUseCase;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var getCountryStatus = await GetCountryUseCase.ExecuteAsync(id.Value);
            if (getCountryStatus != null)
            {
                Country = getCountryStatus;
                return Page();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
