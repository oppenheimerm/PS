using PA.Web.API.Authorization.Interfaces;
using PA.Web.API.Repositories;

namespace PA.Web.API.Authorization.MiddleWare
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, WebApiUserRepository WebApiUserRepository, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation



                //var user = WebApiUserRepository.GetMemberByIdAsync(userId.Value);
                context.Items["User"] = await WebApiUserRepository.GetMemberByIdAsync(userId.Value);//userService.GetById(userId.Value);
            }

            await _next(context);
        }
    }
}
