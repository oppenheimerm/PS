using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PS.Core.Models;

namespace PS.API.Authorization.Attributes
{
    /// <summary>
    ///  Custom [Authorize] attribute is added to controller classes or action methods that require 
    ///  the <see cref="Member"/> to be authenticated.  On successful authorization no action is 
    ///  taken and the request is passed through to the controller action method, if 
    ///  authorization fails a 401 Unauthorized response is returned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var user = context.HttpContext.Items["User"];
            var account = (Member)context.HttpContext.Items["User"];
            if (account == null)
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
