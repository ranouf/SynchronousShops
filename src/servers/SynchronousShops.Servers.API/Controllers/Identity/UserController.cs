using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SynchronousShops.Domains.Core.Identity;
using SynchronousShops.Domains.Core.Identity.Entities;
using SynchronousShops.Libraries.Constants;
using SynchronousShops.Libraries.Extensions;
using SynchronousShops.Libraries.Session;
using SynchronousShops.Servers.API.Attributes;
using SynchronousShops.Servers.API.Controllers.Dtos;
using SynchronousShops.Servers.API.Controllers.Identity.Dtos;
using SynchronousShops.Servers.API.Filters.Dtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SynchronousShops.Servers.API.Controllers.Identity
{
    [Route(Api.V1.User.Url)]
    [AuthorizeAdministrators]
    public class UserController : AuthentifiedBaseController
    {
        private readonly IRoleManager _roleManager;

        public UserController(
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
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [AuthorizeAdministratorAndManagers]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid id)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(GetUserByIdAsync)}, currentUser:{currentUser.ToJson()}, id:{id}");
            var user = await _userManager.FindByIdAsync(id);
            ValidateUserExists(user, id);
            return new ObjectResult(Mapper.Map<User, UserDto>(user));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<UserDto>), 200)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [AuthorizeAdministrators]
        public async Task<IActionResult> GetUsersAsync([FromQuery] FilterRequestDto dto)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(GetUsersAsync)}, currentUser:{currentUser.ToJson()}, dto:{dto.ToJson()}");
            var result = await _userManager.GetAllAsync(
                dto.Filter
            );

            return new ObjectResult(Mapper.Map<IList<User>, IList<UserDto>>(result));
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [AuthorizeAdministrators]
        public async Task<IActionResult> InviteUserAsync([FromBody] InviteUserRequestDto dto)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(InviteUserAsync)}, currentUser:{currentUser.ToJson()}, dto:{dto.ToJson()}");

            var role = await _roleManager.FindByIdAsync(dto.RoleId);
            ValidateRoleExists(role, dto, currentUser);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                user = new User(
                  dto.Email,
                  dto.Firstname,
                  dto.Lastname,
                  invitedByUser: currentUser
                );
                user = await _userManager.CreateAsync(user, role);
            }

            user = await _userManager.InviteAsync(user, role);
            return new ObjectResult(Mapper.Map<User, UserDto>(user));
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [AuthorizeAdministrators]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid id, [FromBody] UpdateUserRequestDto dto)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(UpdateUserAsync)}, currentUser:{currentUser.ToJson()}, dto:{dto.ToJson()}");

            var user = await _userManager.FindByIdAsync(id);
            ValidateUserExists(user, id);

            var role = await _roleManager.FindByIdAsync(dto.RoleId);
            ValidateRoleExists(role, dto, currentUser);

            user.Update(
                dto.Firstname,
                dto.Lastname
            );
            user.SetRole(role); //TODO To validate

            user = await _userManager.UpdateAsync(user);
            return new ObjectResult(Mapper.Map<User, UserDto>(user));
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(AllowToLoginAsync)}, currentUser:{currentUser.ToJson()}, id:{id}");
            if (currentUser.Id == id)
            {
                Logger.LogWarning($"{nameof(AllowToLoginAsync)}, Can't delete himself, currentUser:{currentUser.ToJson()}, id:{id}");
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(id);
            ValidateUserExists(user, id);

            await _userManager.DeleteAsync(user);

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Route(Api.V1.User.Lock)]
        public async Task<IActionResult> LockUserAsync([FromRoute] Guid id)
        {
            return await AllowToLoginAsync(id, false);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Route(Api.V1.User.Unlock)]
        public async Task<IActionResult> UnlockUserAsync([FromRoute] Guid id)
        {
            return await AllowToLoginAsync(id, true);
        }

        #region Private
        private async Task<IActionResult> AllowToLoginAsync(Guid id, bool allow)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(AllowToLoginAsync)},currentUser:{currentUser.ToJson()}, id:{id}, allow:{allow}");
            if (currentUser.Id == id)
            {
                Logger.LogWarning($"{nameof(AllowToLoginAsync)}, Can't allow himself, currentUser:{currentUser.ToJson()}, id:{id}");
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                Logger.LogWarning($"{nameof(AllowToLoginAsync)}, User not found, currentUser:{currentUser.ToJson()}, id:{id}");
                return NotFound();
            }

            await _userManager.AllowUserToLoginAsync(user, allow);

            return Ok();
        }

        private void ValidateRoleExists(Role role, IDto dto, User currentUser)
        {
            if (role == null)
            {
                Logger.LogWarning($"Role not found, currentUser:{currentUser.ToJson()}, dto:{dto.ToJson()}");
                throw new KeyNotFoundException();
            }
        }

        private void ValidateUserExists(User user, IDto dto)
        {
            if (user == null)
            {
                Logger.LogWarning($"User not found, dto:{dto.ToJson()}");
                throw new KeyNotFoundException();
            }
        }

        private void ValidateUserExists(User user, Guid id)
        {
            if (user == null)
            {
                Logger.LogWarning($"User not found, id:{id}");
                throw new KeyNotFoundException();
            }
        }
        #endregion
    }
}
