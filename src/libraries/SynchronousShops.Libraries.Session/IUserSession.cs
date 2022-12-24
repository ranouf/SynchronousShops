using System;

namespace SynchronousShops.Libraries.Session
{
    public interface IUserSession
    {
        Guid? UserId { get; }
        string BaseUrl { get; }
    }
}
