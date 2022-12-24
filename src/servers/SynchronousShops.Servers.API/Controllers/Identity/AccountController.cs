using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SynchronousShops.Domains.Core.Identity;
using SynchronousShops.Domains.Core.Identity.Entities;
using SynchronousShops.Libraries.Constants;
using SynchronousShops.Libraries.Extensions;
using SynchronousShops.Libraries.Session;
using SynchronousShops.Servers.API.Controllers.Identity.Dtos;
using SynchronousShops.Servers.API.Filters.Dtos;
using System.Net;
using System.Threading.Tasks;

namespace SynchronousShops.Servers.API.Controllers.Identity
{
    [Route(Api.V1.Account.Url)]
    [Authorize]
    [ApiController]
    public class AccountController : AuthentifiedBaseController
    {
        public AccountController(
          IUserManager userManager,
          IMapper mapper,
          IUserSession session,
          ILogger<AccountController> logger
        ) : base(session, userManager, mapper, logger)
        {
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Authorize]
        [Route(Api.V1.Account.Password)]
        public async Task<IActionResult> ChangePaswordAsync([FromBody] ChangePasswordRequestDto dto)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(ChangePaswordAsync)}, current:{currentUser.ToJson()}, dto: {dto.ToJson()}");
            await _userManager.ChangePasswordAsync(await GetCurrentUserAsync(), dto.CurrentPassword, dto.NewPassword);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Authorize]
        [Route(Api.V1.Account.Profile)]
        public async Task<IActionResult> UpdateProfileAsync([FromBody] ChangeProfileRequestDto dto)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(UpdateProfileAsync)}, current:{currentUser.ToJson()}, dto: {dto.ToJson()}");
            currentUser.Update(dto.Firstname, dto.Lastname);
            currentUser = await _userManager.UpdateAsync(currentUser);
            return new ObjectResult(Mapper.Map<User, UserDto>(currentUser));
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Authorize]
        [Route(Api.V1.Account.Profile)]
        public async Task<IActionResult> GetProfileAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(GetProfileAsync)}, current:{currentUser.ToJson()}");
            var result = await _userManager.FindByIdAsync(currentUser.Id);
            return new ObjectResult(Mapper.Map<User, UserDto>(result));
        }
    }
}
