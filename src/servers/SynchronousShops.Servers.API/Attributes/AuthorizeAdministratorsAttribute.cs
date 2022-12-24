using SynchronousShops.Domains.Core;
using Microsoft.AspNetCore.Authorization;

namespace SynchronousShops.Servers.API.Attributes
{
    public class AuthorizeAdministratorsAttribute : AuthorizeAttribute
    {
        public AuthorizeAdministratorsAttribute()
        {
            Roles = Constants.Roles.Administrator;
        }
    }
}
