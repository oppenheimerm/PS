using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.UseCases.CountryUseCase;
using PS.UseCases.Interfaces;
using PS.Web.Admin.ViewModels;

namespace PS.Web.Admin.Pages.Stations
{
    public class CreateModel : PageModel
    {
        public readonly IAddPetrolStationUseCase AddPetrolStationUseCase;
        public readonly IGetAllCountriesUseCase GetAllCountriesUseCase;
        public readonly IGetAllVendorsUseCase GetAllVendorsUseCase;
        private readonly IConfiguration Configuration;
        private readonly IGetCountryCodeByIdUseCase GetCountryCodeByIdUseCase;

        public CreateModel(IAddPetrolStationUseCase addPetrolStationUseCase, IGetAllCountriesUseCase getAllCountriesUseCase, 
            IConfiguration configuration, IGetAllVendorsUseCase getAllVendorsUseCase, IGetCountryCodeByIdUseCase getCountryCodeByIdUseCase)
        {
            AddPetrolStationUseCase = addPetrolStationUseCase;
            GetAllCountriesUseCase = getAllCountriesUseCase;
            Configuration = configuration;
            GetAllVendorsUseCase = getAllVendorsUseCase;
            GoogleApiKey = Configuration["GoogleServices:ApiKey"];
            GetCountryCodeByIdUseCase = getCountryCodeByIdUseCase;
        }


        [BindProperty]
        public AddPetrolStationVM AddPetrolStationVM { get; set; } = default!;
        public string GoogleApiKey;

        public void OnGet()
        {
            AddPetrolStationVM = new();
            AddPetrolStationVM.Countries = GetAllCountriesUseCase.Execute().ToList();
            AddPetrolStationVM.Vendors = GetAllVendorsUseCase.Execute().ToList();
            
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || AddPetrolStationVM == null)
            {
                return Page();
            }

            // Add country code
            var countryCode = await GetCountryCodeByIdUseCase.ExecuteAsync(AddPetrolStationVM.CountryId.Value);
            AddPetrolStationVM.CountryCode = countryCode.ToUpper();
            var status = await AddPetrolStationUseCase.ExecuteAsync(AddPetrolStationVM.ToStationVM());
            if (status.success)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return Page();
            }

        }
    }
}
