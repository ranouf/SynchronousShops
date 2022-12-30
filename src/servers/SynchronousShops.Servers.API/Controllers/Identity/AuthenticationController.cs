
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SynchronousShops.Domains.Core.Identity;
using SynchronousShops.Domains.Core.Identity.Entities;
using SynchronousShops.Domains.Core.Session;
using SynchronousShops.Libraries.Authentication;
using SynchronousShops.Libraries.Constants;
using SynchronousShops.Libraries.Extensions;
using SynchronousShops.Servers.API.Controllers.Dtos;
using SynchronousShops.Servers.API.Controllers.Identity.Dtos;
using SynchronousShops.Servers.API.Filters.Dtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SynchronousShops.Servers.API.Controllers.Identity
{
    [Route(Api.V1.Authentication.Url)]
    [ApiController]
    public class AuthenticationController : AuthentifiedBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(
          IAuthenticationService authenticationService,
          IUserManager userManager,
          IUserSession session,
          IMapper mapper,
          ILogger<AuthenticationController> logger
        ) : base(session, userManager, mapper, logger)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Route(Api.V1.Authentication.Register)]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegistrationRequestDto dto)
        {
            Logger.LogInformation($"{nameof(RegisterUserAsync)}, dto:{dto.ToJson()}");
            var userToRegister = new User(
              dto.Email,
              dto.Firstname,
              dto.Lastname
            );

            await _userManager.RegisterAsync(userToRegister, dto.Password);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Route(Api.V1.Authentication.ResendEmailConfirmation)]
        public async Task<IActionResult> ResendEmailConfirmationAsync([FromBody] ResendEmailConfirmationRequestDto dto)
        {
            Logger.LogInformation($"{nameof(RegisterUserAsync)}, dto:{dto.ToJson()}");
            var user = await _userManager.FindByEmailAsync(dto.Email);

            await ValidateEmailConfirmationNeeded(user, dto);

            await _userManager.ReSendEmailConfirmationAsync(user);

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.Unauthorized)]
        [Route(Api.V1.Authentication.ConfirmRegistrationEmail)]
        public async Task<IActionResult> ConfirmRegistrationEmailAsync([FromBody] ConfirmRegistrationEmailRequestDto dto)
        {
            Logger.LogInformation($"{nameof(ConfirmRegistrationEmailAsync)}, dto:{dto.ToJson()}");
            var user = await _userManager.FindByEmailAsync(dto.Email);

            await ValidateEmailConfirmationNeeded(user, dto);

            await _userManager.ConfirmRegistrationEmailAsync(user, dto.Token);

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.Unauthorized)]
        [Route(Api.V1.Authentication.ConfirmInvitationEmail)]
        public async Task<IActionResult> ConfirmInvitationEmailAsync([FromBody] ConfirmInvitationEmailRequestDto dto)
        {
            Logger.LogInformation($"{nameof(ConfirmInvitationEmailAsync)}, dto:{dto.ToJson()}");
            var user = await _userManager.FindByEmailAsync(dto.Email);

            await ValidateEmailConfirmationNeeded(user, dto);

            await _userManager.ConfirmInvitationEmailAsync(user, dto.Password, dto.Token);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.Unauthorized)]
        [Route(Api.V1.Authentication.Login)]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginRequestDto dto)
        {
            Logger.LogInformation($"{nameof(LoginUserAsync)}, dto:{dto.ToJson()}");
            var user = await _userManager.FindByEmailAsync(dto.Email);

            ValidateUserAllowed(user, dto);
            await ValidateEmailConfirmed(user, dto);
            ValidateUserNotLocked(user, dto);
            await ValidatePasswordsMatch(user, dto.Password, dto);

            return new ObjectResult(new LoginResponseDto
            {
                CurrentUser = Mapper.Map<User, UserDto>(user),
                Token = _authenticationService.GenerateToken(
                    new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.RoleName),
                    })
                )
            });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route(Api.V1.Authentication.PasswordForgotten)]
        public async Task<IActionResult> PasswordForgottenAsync([FromBody] PasswordForgottenRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            ValidateUserExists(user, dto);

            await _userManager.PasswordForgottenAsync(user);

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route(Api.V1.Authentication.ResetPassword)]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            ValidateUserExists(user, dto);

            await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

            return Ok();
        }

        #region Private
        private async Task ValidateEmailConfirmationNeeded(User user, IDto dto)
        {
            ValidateUserExists(user, dto);
            await ValidateEmailNotConfirmed(user, dto);
            ValidateUserNotLocked(user, dto);
        }

        private void ValidateUserNotLocked(User user, IDto dto)
        {
            if (user.LockoutEnabled)
            {
                Logger.LogWarning($"User locked, dto:{dto.ToJson()}, user: {user.ToJson()}");
                throw new UnauthorizedAccessException("Accound locked");
            }
        }

        private async Task ValidateEmailConfirmed(User user, IDto dto)
        {
            if (!await _userManager.CanSignInAsync(user))
            {
                Logger.LogWarning($"User Email not confirmed, dto:{dto.ToJson()}, user: {user.ToJson()}");
                throw new UnauthorizedAccessException("Email not confirmed");
            }
        }

        private async Task ValidateEmailNotConfirmed(User user, IDto dto)
        {
            if (await _userManager.CanSignInAsync(user))
            {
                Logger.LogWarning($"User Email already confirmed, dto:{dto.ToJson()}, user: {user.ToJson()}");
                throw new UnauthorizedAccessException("Email already confirmed");
            }
        }

        private void ValidateUserAllowed(User user, IDto dto)
        {
            if (user == null)
            {
                Logger.LogWarning($"User not found, dto:{dto.ToJson()}");
                throw new UnauthorizedAccessException();
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

        private async Task ValidatePasswordsMatch(User user, string password, IDto dto)
        {
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                Logger.LogWarning($"Login failed, dto:{dto.ToJson()}, user: {user.ToJson()}");
                throw new UnauthorizedAccessException();
            }
        }
        #endregion
    }
}
