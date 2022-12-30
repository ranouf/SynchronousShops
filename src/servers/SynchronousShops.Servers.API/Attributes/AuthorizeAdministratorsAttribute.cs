using SynchronousShops.Domains.Core;

namespace SynchronousShops.Servers.API.Attributes
{
    public class AuthorizeAdministratorsAttribute : AuthorizeAttribute
    {
        public AuthorizeAdministratorsAttribute() : base()
        {
            Roles = Constants.Roles.Administrator;
        }
    }
}
