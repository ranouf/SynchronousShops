using System.Security.Claims;

namespace SynchronousShops.Libraries.Authentication
{
    public interface IAuthenticationService
    {
        string GenerateToken(ClaimsIdentity claimsIdentity);
    }
}
