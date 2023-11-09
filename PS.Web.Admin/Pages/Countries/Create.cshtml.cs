using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.UseCases.Interfaces;
using PS.Web.Admin.ViewModels;

namespace PS.Web.Admin.Pages.Countries
{
    public class CreateModel : PageModel
    {
        public readonly IAddCountryUseCase AddCountryUseCase;
		public readonly IIsCountryCodeUniqueUseCase IsCountryCodeUniqueUseCase;
		public CreateModel(IAddCountryUseCase addCountryUseCase, IIsCountryCodeUniqueUseCase isCountryCodeUniqueUseCase)
        {
            AddCountryUseCase = addCountryUseCase;
            IsCountryCodeUniqueUseCase = isCountryCodeUniqueUseCase;
        }

        [BindProperty]
        public AddCountryVM AddCountryVM { get; set; } = default!;

        public void OnGet()
        {
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || AddCountryVM == null)
            {
                return Page();
            }

			var countryCodeUnique = await IsCountryCodeUniqueUseCase.ExecuteAsync(AddCountryVM.CountryCode);
			if (countryCodeUnique)
            {
				var status = await AddCountryUseCase.ExecuteAsync(AddCountryVM.ToCountryVM());
				if (status.success)
				{
					return RedirectToPage("./Index");
				}
				else
				{
					return Page();
				}
			}
            else
            {
				// Error, Country code in use, return form
				ModelState.AddModelError(string.Empty, $"Country code: {AddCountryVM.CountryCode.ToUpperInvariant()} already in use.");
				return Page();
			}

        }
    }
}
