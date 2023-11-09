using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.Core.Models;
using PS.UseCases.CountryUseCase;
using PS.UseCases.Interfaces;

namespace PS.Web.Admin.Pages.Stations
{
    public class DetailsModel : PageModel
    {
        public readonly IGetPetrolStationByIdUseCase GetPetrolStationByIdUseCase;
        public readonly IGetCountryUseCase GetCountryUseCase;

        public DetailsModel(
            IGetPetrolStationByIdUseCase getPetrolStationByIdUseCase,
            IGetCountryUseCase getCountryUseCase
            )
        {
            GetPetrolStationByIdUseCase = getPetrolStationByIdUseCase;
            GetCountryUseCase = getCountryUseCase;
        }

        public Station Station { get; set; } = default!;
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
                Station = getStationStatus;
                CountryRequest = await GetCountryUseCase.ExecuteAsync(Station.CountryId.Value);
                return Page();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
