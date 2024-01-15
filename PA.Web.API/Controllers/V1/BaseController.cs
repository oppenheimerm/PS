using Microsoft.AspNetCore.Mvc;
using PA.Core.Models;

namespace PA.Web.API.Controllers.V1
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        // returns the current authenticated account (null if not logged in)

        /// <summary>
        /// Reutns the current authenticated account(<see cref="Member"/>) (null if not logged in
        /// </summary>
        public Member? Account => HttpContext.Items["Account"] as Member;
    }
}
