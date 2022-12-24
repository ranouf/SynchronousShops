using System.Collections.Generic;

namespace SynchronousShops.Servers.API.SignalR.Connection
{
    public interface IConnectionService
    {
        void Add(string userId, string connectionId);
        void Remove(string userId);
        IReadOnlyList<string> GetAllExcept(string userId);
        string FindByUserId(string userId);
    }
}