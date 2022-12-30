using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SynchronousShops.Domains.Core.Identity;
using SynchronousShops.Domains.Core.Session;

namespace SynchronousShops.Servers.API.Controllers
{
    [ApiController]
    public abstract class AuthentifiedBaseController : BaseController
    {
        internal readonly IUserManager _userManager;
        internal IUserSession Session { get; set; }

        public AuthentifiedBaseController(
            IUserSession session,
            IUserManager userManager,
            IMapper mapper,
            ILogger logger
        ) : base(mapper, logger)
        {
            _userManager = userManager;
            Session = session;
        }
    }
}
