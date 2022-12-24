using MimeKit;
using System;
using System.Threading.Tasks;

namespace SynchronousShops.Libraries.SMTP.SmtpClients
{
    public interface ISmtpClient : IDisposable
    {
        bool IsConnected { get; }
        Task ConnectAsync(string host, int port, bool useSsl);
        Task AuthenticateAsync(string username, string password);
        Task SendAsync(MimeMessage message);
    }
}
