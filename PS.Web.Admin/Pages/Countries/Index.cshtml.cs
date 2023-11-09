using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.UseCases.Interfaces;

namespace PS.Web.Admin.Pages.Countries
{
    public class IndexModel : PageModel
    {
        public readonly IGetAllCountriesUseCase GetAllCountriesUseCase;
        public IList<Core.Models.Country> Countries { get; set; } = default!;
        public IndexModel(IGetAllCountriesUseCase getAllCountriesUseCase)
        {
            GetAllCountriesUseCase = getAllCountriesUseCase;
        }

        public void OnGet()
        {
            Countries = GetAllCountriesUseCase.Execute().ToList();
        }
    }
}
