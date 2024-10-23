using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using WEBAPP_NATURPIURA.Servicios;

namespace WEBAPP_NATURPIURA.Filtros
{
    public class AuthorizeRoleFilter : IAsyncActionFilter
    {
        private readonly string _role;
        private readonly ServicioRol _roleService;

        public AuthorizeRoleFilter(string role, ServicioRol roleService)
        {
            _role = role;
            _roleService = roleService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !await _roleService.IsUserInRoleAsync(userId, _role))
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
}
