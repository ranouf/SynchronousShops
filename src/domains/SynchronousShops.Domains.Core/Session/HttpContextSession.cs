﻿using Microsoft.AspNetCore.Http;
using SynchronousShops.Domains.Core.Identity.Entities;
using System;
using System.Linq;
using System.Security.Claims;

namespace SynchronousShops.Domains.Core.Session
{
    public class HttpContextSession : IUserSession
    {
        private readonly IHttpContextAccessor _context;

        public Guid? UserId
        {
            get
            {
                // During Integration Tests seeding, the HttpContext is null
                if (_context.HttpContext == null)
                {
                    return null;
                }

                var result = Guid.Empty;
                var userId = _context.HttpContext.User.Claims
                  .Where(c => c.Type == ClaimTypes.NameIdentifier)
                  .Select(c => c.Value)
                  .FirstOrDefault();

                if (Guid.TryParse(userId, out result))
                {
                    return result;
                }
                return null;
            }
        }
        public string BaseUrl
        {
            get
            {
                // During Integration Tests seeding, the HttpContext is null
                if (_context.HttpContext == null)
                {
                    return null;
                }

                return $"{_context.HttpContext.Request.Scheme}://{_context.HttpContext.Request.Host.Value}";
            }
        }

        public User CurrentUser { get; set; }

        public HttpContextSession(IHttpContextAccessor context)
        {
            _context = context;
        }
    }
}