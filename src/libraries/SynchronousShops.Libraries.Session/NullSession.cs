using System;

namespace SynchronousShops.Libraries.Session
{
    public class NullSession : IUserSession
    {
        public Guid? UserId => null;

        public string BaseUrl => null;
    }
}