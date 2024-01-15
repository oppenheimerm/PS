using Microsoft.AspNetCore.Mvc;

namespace PA.Web.API.Controllers.V1
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DefaultController : Controller
    {
        [Route("/")]
        [Route("/docs")]
        [Route("/swagger")]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger/index.html");
        }
    }
}
