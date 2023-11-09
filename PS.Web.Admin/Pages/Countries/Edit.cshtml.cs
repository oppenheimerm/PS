using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.Core.Models;
using PS.UseCases.CountryUseCase;
using PS.UseCases.Interfaces;

namespace PS.Web.Admin.Pages.Countries
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public PS.Core.Models.Country? Country { get; set; } = default;

        public readonly IGetCountryUseCase GetCountryUseCase;
        public readonly IEditCountryUseCase EditCountryUseCase;
        public readonly IIsCountryCodeUniqueUseCase IsCountryCodeUniqueUseCase;

        public EditModel(IEditCountryUseCase editCountryUseCase, IGetCountryUseCase getCountryUseCase,
            IIsCountryCodeUniqueUseCase isCountryCodeUniqueUseCase)
        {
            GetCountryUseCase = getCountryUseCase;
            EditCountryUseCase = editCountryUseCase;
            IsCountryCodeUniqueUseCase = isCountryCodeUniqueUseCase;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var country = await GetCountryUseCase.ExecuteAsync(id);
            if (country is not null)
            {
                Country = country;
                return Page();
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Country is null)
            {
                return Page();
            }

            var countryCodeUnique = await IsCountryCodeUniqueUseCase.ExecuteAsync(Country.CountryCode);
            if (countryCodeUnique)
            {
				var countryToEdit = await GetCountryUseCase.ExecuteAsync(Country.Id);
				if (countryToEdit is not null)
				{
					//	We only want to update
					//		CountryName, CountryCode
					countryToEdit.CountryName = Country.CountryName;
					countryToEdit.CountryCode = Country.CountryCode.ToUpperInvariant();

					var status = await EditCountryUseCase.ExecuteAsync(countryToEdit);
					if (status.success)
					{
						return RedirectToPage("./Index");
					}
					else
					{
						Country = countryToEdit;
						return Page();
					}
				}
				else
				{
					return NotFound();
				}
			}
            else
            {
				// Error, Country code in use, return form
				ModelState.AddModelError(string.Empty, $"Country code: {Country.CountryCode.ToUpperInvariant()} already in use.");
                return Page();
			}



        }
    }
}
