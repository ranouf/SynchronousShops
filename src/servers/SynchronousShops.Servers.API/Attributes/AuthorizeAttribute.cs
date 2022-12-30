using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SynchronousShops.Domains.Core.Identity;
using SynchronousShops.Domains.Core.Session;
using System.Threading.Tasks;

namespace SynchronousShops.Servers.API.Attributes
{
    public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            var userSession = context.HttpContext.RequestServices.GetRequiredService<IUserSession>();
            if (!user.Identity.IsAuthenticated || !userSession.UserId.HasValue)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userManager = context.HttpContext.RequestServices.GetRequiredService<IUserManager>();
            var curentUser = await userManager.FindByIdAsync(userSession.UserId.Value);
            if (curentUser == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            userSession.CurrentUser = curentUser;
        }
    }
}
