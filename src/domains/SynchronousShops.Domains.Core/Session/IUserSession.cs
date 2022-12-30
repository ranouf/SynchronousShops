using SynchronousShops.Domains.Core.Identity.Entities;
using System;

namespace SynchronousShops.Domains.Core.Session
{
    public interface IUserSession
    {
        Guid? UserId { get; }
        string BaseUrl { get; }
        User CurrentUser { get; set; }
    }
}
