using System.Threading.Tasks;

namespace SynchronousShops.Libraries.SMTP
{
    public interface ISmtpService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}