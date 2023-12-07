using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.Core.Models;
using PS.UseCases.CountryUseCase;
using PS.UseCases.Interfaces;
using PS.UseCases.PetrolStationUseCase;
using PS.UseCases.VendorUseCase;
using PS.Web.Admin.ViewModels;

namespace PS.Web.Admin.Pages.Stations
{
    public class EditModel : PageModel
    {
		public readonly IGetPetrolStationByIdUseCase GetPetrolStationByIdUseCase;
        public readonly IGetAllCountriesUseCase GetAllCountriesUseCase;
        public readonly IGetAllVendorsUseCase GetAllVendorsUseCase;
        private readonly IGetCountryCodeByIdUseCase GetCountryCodeByIdUseCase;
        private readonly IConfiguration Configuration;
        private readonly IEditPterolStationUseCase EditPetrolStationUseCase;
        public string GoogleApiKey;

        public EditModel(IGetPetrolStationByIdUseCase getPetrolStationByIdUseCase,
            IGetAllCountriesUseCase getAllCountriesUseCase, IConfiguration configuration,
			IGetAllVendorsUseCase getAllVendorsUseCase, IGetCountryCodeByIdUseCase getCountryCodeByIdUseCase,
            IEditPterolStationUseCase editPterolStationUseCase)
        {
            GetPetrolStationByIdUseCase = getPetrolStationByIdUseCase;
            GetAllCountriesUseCase = getAllCountriesUseCase;
			GetAllVendorsUseCase = getAllVendorsUseCase;
            GetCountryCodeByIdUseCase = getCountryCodeByIdUseCase;
            Configuration = configuration;
            GoogleApiKey = Configuration["GoogleServices:ApiKey"];
            EditPetrolStationUseCase = editPterolStationUseCase;
        }

		[BindProperty]
		public EditPetrolStationVM EditPetrolStationVM { get; set; } = default!;
		public Country? CountryRequest { get; set; } = default!;   

		public async Task<IActionResult> OnGet(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

            var getStationStatus = await GetPetrolStationByIdUseCase.ExecuteAsync(id.Value);
			if (getStationStatus != null)
			{
                EditPetrolStationVM = getStationStatus.ToEditStationVM();
                EditPetrolStationVM.Countries = GetAllCountriesUseCase.Execute().ToList();
                EditPetrolStationVM.Vendors = GetAllVendorsUseCase.Execute().ToList();
                return Page();
			}
			else
			{
				return NotFound();
			}
		}

		public async Task<IActionResult> OnPostAsync()
		{
            var stationToEdit = await GetPetrolStationByIdUseCase.ExecuteAsync(EditPetrolStationVM.Id.Value);
            if (!ModelState.IsValid || EditPetrolStationVM == null || stationToEdit == null)
            {
                return Page();
            }

            // Add country code
            var countryCode = await GetCountryCodeByIdUseCase.ExecuteAsync(EditPetrolStationVM.CountryId.Value);
            EditPetrolStationVM.CountryCode = countryCode.ToUpper();
            //  prevent overposting
            // TODO - Need to clean this code up
            stationToEdit.StationName = EditPetrolStationVM.StationName;
            stationToEdit.StationAddress = EditPetrolStationVM.StationAddress;
            stationToEdit.StationPostcode = EditPetrolStationVM.StationPostcode;
            stationToEdit.Latitude = EditPetrolStationVM.Latitude;
            stationToEdit.Longitude = EditPetrolStationVM.Longitude;
            stationToEdit.VendorId = EditPetrolStationVM.VendorId;
            stationToEdit.CountryId = EditPetrolStationVM.CountryId;
            stationToEdit.CountryCode = EditPetrolStationVM.CountryCode;


            var status = await EditPetrolStationUseCase.ExecuteAsync(stationToEdit);
            if (status.success)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                var getStationStatus = await GetPetrolStationByIdUseCase.ExecuteAsync(EditPetrolStationVM.ToStation().Id);
                EditPetrolStationVM = getStationStatus.ToEditStationVM();
                EditPetrolStationVM.Countries = GetAllCountriesUseCase.Execute().ToList();
                EditPetrolStationVM.Vendors = GetAllVendorsUseCase.Execute().ToList();
                return Page();
            }
        }

    }
}
