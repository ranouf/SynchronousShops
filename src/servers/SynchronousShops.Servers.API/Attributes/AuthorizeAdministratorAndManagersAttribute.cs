using SynchronousShops.Domains.Core;

namespace SynchronousShops.Servers.API.Attributes
{
    public class AuthorizeAdministratorAndManagersAttribute : AuthorizeAttribute
    {
        public AuthorizeAdministratorAndManagersAttribute()
        {
            Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}";
        }
    }
}
