using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.Core.Models;
using PS.UseCases.Interfaces;

namespace PS.Web.Admin.Pages.Members
{
    public class IndexModel : PageModel
    {
        //public IList<Station> Stations { get; set; } = default!;
        public IList<Member> Members { get; set; } = default!;
        public readonly IGetAllMembersUseCase GetAllMembersUseCase;

        public IndexModel(IGetAllMembersUseCase getAllMembersUseCase)
        {
            GetAllMembersUseCase = getAllMembersUseCase;
        }

        public void OnGet()
        {
            Members = GetAllMembersUseCase.Execute().ToList();
        }
    }
}
