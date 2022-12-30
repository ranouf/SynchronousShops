using SynchronousShops.Domains.Core.Identity.Entities;
using System;

namespace SynchronousShops.Domains.Core.Session
{
    public class NullSession : IUserSession
    {
        public Guid? UserId => null;

        public string BaseUrl => null;

        public User CurrentUser { get; set; }
    }
}