using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SynchronousShops.Domains.Core.Identity;
using SynchronousShops.Domains.Core.Identity.Entities;
using SynchronousShops.Domains.Core.Session;
using SynchronousShops.Libraries.Constants;
using SynchronousShops.Libraries.Extensions;
using SynchronousShops.Servers.API.Attributes;
using SynchronousShops.Servers.API.Controllers.Identity.Dtos;
using SynchronousShops.Servers.API.Filters.Dtos;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SynchronousShops.Servers.API.Controllers.Identity
{
    [Route(Api.V1.Role.Url)]
    [AuthorizeAdministrators]
    public class RoleController : AuthentifiedBaseController
    {
        private readonly IRoleManager _roleManager;

        public RoleController(
            IUserSession session,
            IUserManager userManager,
            IRoleManager roleManager,
            ILogger<UserController> logger,
            IMapper mapper
        ) : base(session, userManager, mapper, logger)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RoleDto>), 200)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var currentUser = Session.CurrentUser;
            Logger.LogInformation($"{nameof(GetAllRolesAsync)}, currentUser:{currentUser.ToJson()}");
            var result = await _roleManager.GetAllAsync();
            return new ObjectResult(Mapper.Map<IEnumerable<Role>, IEnumerable<RoleDto>>(result));
        }
    }
}
