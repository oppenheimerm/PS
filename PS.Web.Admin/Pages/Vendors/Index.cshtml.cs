using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.Core.Models;
using PS.UseCases.Interfaces;

namespace PS.Web.Admin.Pages.Vendors
{
    public class IndexModel : PageModel
    {
        public readonly IGetAllVendorsUseCase GetAllVendorsUseCase;
        public IList<Vendor> Vendors { get; set; } = default!;

        public IndexModel(IGetAllVendorsUseCase getAllVendorsUseCase)
        {
            GetAllVendorsUseCase = getAllVendorsUseCase;
        }
        public void OnGet()
        {
            //  TODO - Add paging
            Vendors = GetAllVendorsUseCase.Execute().ToList();
        }
    }
}
