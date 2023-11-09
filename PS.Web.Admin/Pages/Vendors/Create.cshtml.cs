using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.UseCases.Interfaces;
using PS.UseCases.VendorUseCase;
using PS.Web.Admin.ViewModels;

namespace PS.Web.Admin.Pages.Vendors
{    
    public class CreateModel : PageModel
    {
        public readonly IAddVendorUseCase AddVendorUseCase;
        public readonly IIsVendorCodeUniqueUseCase IsVendorCodeUniqueUseCase;
		public readonly IGetAllCountriesUseCase GetAllCountriesUseCase;

		[BindProperty]        
        public AddVendorVM? AddVendorVM { get; set; } = default;

		public CreateModel(IAddVendorUseCase addVendorUseCase, IIsVendorCodeUniqueUseCase isVendorCodeUniqueUseCase,
			IGetAllCountriesUseCase getAllCountriesUseCase)
        {
            AddVendorUseCase = addVendorUseCase;
            IsVendorCodeUniqueUseCase = isVendorCodeUniqueUseCase;
			GetAllCountriesUseCase = getAllCountriesUseCase;
        }

        public IActionResult OnGet()
        {
			AddVendorVM = new();
			AddVendorVM.Countries = GetAllCountriesUseCase.Execute().ToList();
			return Page();
        }

		// To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
		public async Task<IActionResult> OnPostAsync()
        {
			if (!ModelState.IsValid || AddVendorVM == null)
			{
                AddVendorVM = new();
                AddVendorVM.Countries = GetAllCountriesUseCase.Execute().ToList();
                return Page();
			}
			var vendorCodeUnique = await IsVendorCodeUniqueUseCase.ExecuteAsync(AddVendorVM.VendorCode);
            if (vendorCodeUnique)
            {
				var status = await AddVendorUseCase.ExecuteAsync(AddVendorVM.ToVendorVM());
				if (status.success)
				{
					return RedirectToPage("./Index");
				}
				else
				{
                    AddVendorVM = new();
                    AddVendorVM.Countries = GetAllCountriesUseCase.Execute().ToList();
                    return Page();
                }
			}
            else
            {
				// Error, Vendor code in use, return form
				ModelState.AddModelError(string.Empty, $"Vendor code: {AddVendorVM.VendorCode.ToUpperInvariant()} already in use.");
                AddVendorVM = new();
                AddVendorVM.Countries = GetAllCountriesUseCase.Execute().ToList();
                return Page();
            }
		}

	}
}
