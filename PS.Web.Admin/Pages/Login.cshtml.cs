using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PS.Web.Admin.Pages
{

	//  Client IP safelist for ASP.NET Core
	//  https://learn.microsoft.com/en-us/aspnet/core/security/ip-safelist?view=aspnetcore-7.0
	[AllowAnonymous]
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
