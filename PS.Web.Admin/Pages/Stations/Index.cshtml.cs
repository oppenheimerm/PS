using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.Core.Models;
using PS.UseCases.Interfaces;

namespace PS.Web.Admin.Pages.Stations
{
    public class IndexModel : PageModel
    {
        public IList<Station> Stations { get; set; } = default!;

        public readonly IGetAllPetrolStationsUseCase GetAllPetrolStationsUseCase;

        public IndexModel(IGetAllPetrolStationsUseCase getAllPetrolStationsUseCase)
        {
            GetAllPetrolStationsUseCase = getAllPetrolStationsUseCase;
        }

        public void OnGet()
        {
            Stations = GetAllPetrolStationsUseCase.Execute().ToList();
        }
    }
}
