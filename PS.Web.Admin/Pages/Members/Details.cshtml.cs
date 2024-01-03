using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PS.Core.Models;
using PS.UseCases.Interfaces;

namespace PS.Web.Admin.Pages.Members
{
    public class DetailsModel : PageModel
    {
        public readonly IGetMemberByIdUseCase GetMemberByIdUseCase;
        public Member Member { get; set; } = default!;

        public DetailsModel(IGetMemberByIdUseCase getMemberByIdUseCase)
        {
            GetMemberByIdUseCase = getMemberByIdUseCase;
        }

        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var getMemberStatus = await GetMemberByIdUseCase.ExecuteAsync(id.Value);
            if (getMemberStatus != null)
            {
                Member = getMemberStatus;
                return Page();
            }else
            {
                return NotFound();
            }
        }


    }
}
