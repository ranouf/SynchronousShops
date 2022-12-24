using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SynchronousShops.Domains.Core.Emails;
using SynchronousShops.Domains.Core.Identity.Configuration;
using SynchronousShops.Domains.Core.Identity.Entities;
using SynchronousShops.Libraries.EntityFramework.Repositories;
using SynchronousShops.Libraries.EntityFramework.UnitOfWork;
using SynchronousShops.Libraries.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SynchronousShops.Domains.Core.Identity
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRoleManager _roleManager;
        private readonly IEmailManager _emailManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;
        private readonly IdentitySettings _identitySettings;
        private readonly ILogger<UserManager> _logger;

        public UserManager(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IRoleManager roleManager,
            IEmailManager emailManager,
            IUnitOfWork unitOfWork,
            IOptions<IdentitySettings> identitySettings,
            ILogger<UserManager> logger
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailManager = emailManager;
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<User>();
            _identitySettings = identitySettings.Value;
            _logger = logger;
        }

        public async Task<bool> CanSignInAsync(User user)
        {
            return await _signInManager.CanSignInAsync(user);
        }

        public async Task<User> FindByIdAsync(Guid id, bool includeDeleted = false)
        {
            var result = await FindByAsync(u => u.Id == id, includeDeleted);
            if (result != null)
            {
                _logger.LogInformation($"User found: {result.ToJson()}");
            }
            else
            {
                _logger.LogInformation($"User not found with id: {id}");
            }
            return result;
        }

        public async Task<User> FindByEmailAsync(string email, bool includeDeleted = false)
        {
            var result = await FindByAsync(u => u.Email == email, includeDeleted);
            if (result != null)
            {
                _logger.LogInformation($"User found: {result.ToJson()}");
            }
            else
            {
                _logger.LogInformation($"User not found with email: {email}");
            }
            return result;
        }

        public async Task<IList<User>> GetAllAsync(string filter)
        {
            var query = _userRepository.GetAll()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => u);

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(u => u.FullName.Contains(filter) || u.Email.Contains(filter));
            }

            query = query.OrderBy(o => o.FullName);

            return await query.ToListAsync();
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            var role = await _roleManager.FindByNameAsync(Constants.Roles.User);
            var result = await CreateAsync(user, role, password);
            await SendEmailConfirmationAsync(result);

            return result;
        }

        public async Task<User> ReSendEmailConfirmationAsync(User user)
        {
            var result = await SendEmailConfirmationAsync(user);
            return result;
        }

        public async Task<User> CreateAsync(User user, Role role, string password = null)
        {
            var identityResult = password == null
                ? await _userManager.CreateAsync(user)
                : await _userManager.CreateAsync(user, password);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }

            await AddToRoleAsync(user, role);

            return await FindByEmailAsync(user.Email);
        }

        public async Task DeleteAsync(User user)
        {
            var identityResult = await _userManager.DeleteAsync(user);

            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }
        }

        public async Task AllowUserToLoginAsync(User user, bool allow)
        {
            var identityResult = await _userManager.SetLockoutEnabledAsync(user, !allow);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }
        }

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }

        public async Task ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            var identityResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }
            user.GenerateNewSecurityStamp();
            await UpdateAsync(user);
        }

        public async Task<User> UpdateAsync(User user)
        {
            var identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }

            var result = await FindByIdAsync(user.Id);
            return result;
        }

        public async Task PasswordForgottenAsync(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            _logger.LogInformation($"PasswordResetToken is '{token}'");
            await _emailManager.SendPasswordForgottenEmailAsync(user, token);
        }

        public async Task ResetPasswordAsync(User user, string token, string newPassword)
        {
            var identityResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }
        }

        public async Task<User> InviteAsync(User user, Role role)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _logger.LogInformation($"EmailConfirmationToken is '{token}'");
            await _emailManager.SendInviteUserEmailAsync(user, token);
            return user;
        }

        public async Task<User> ConfirmInvitationEmailAsync(User user, string password, string token)
        {
            var identityResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }

            identityResult = await _userManager.RemovePasswordAsync(user);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }

            identityResult = await _userManager.AddPasswordAsync(user, password);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }

            var result = await FindByIdAsync(user.Id);
            return result;
        }

        public async Task ConfirmRegistrationEmailAsync(User user, string token)
        {
            var identityResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }
        }

        #region Private
        private async Task<User> FindByAsync(Expression<Func<User, bool>> where, bool includeDeleted)
        {
            var result = await _userRepository.GetAll()
                .Include(u => u.CreatedByUser)
                .Include(u => u.ModifiedByUser)
                .Include(u => u.DeletedByUser)
                .Include(u => u.InvitedByUser)
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .IgnoreQueryFilters(includeDeleted)
                .FirstOrDefaultAsync(where);
            return result;
        }

        private async Task AddToRoleAsync(User user, Role role)
        {
            var newUser = await _userManager.FindByEmailAsync(user.Email);
            var identityResult = await _userManager.AddToRoleAsync(newUser, role.Name);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }
        }

        private async Task<User> SendEmailConfirmationAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _logger.LogInformation($"EmailConfirmationToken is '{token}'");
            await _emailManager.SendConfirmEmailAsync(user, token);
            return user;
        }
        #endregion
    }
}
