﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SynchronousShops.Domains.Core.Identity;
using SynchronousShops.Domains.Core.Identity.Entities;
using SynchronousShops.Libraries.Session;
using System.Threading.Tasks;

namespace SynchronousShops.Servers.API.Controllers
{
    [ApiController]
    public abstract class AuthentifiedBaseController : BaseController
    {
        internal readonly IUserManager _userManager;
        private User _currentUser;
        internal IUserSession Session { get; set; }
        internal async Task<User> GetCurrentUserAsync()
        {
            if (_currentUser == null && Session.UserId.HasValue)
            {
                _currentUser = await _userManager.FindByIdAsync(Session.UserId.Value);
            }
            return _currentUser;
        }

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
