using Moq;

namespace SynchronousShops.Libraries.SMTP.SmtpClients
{
    public class TestSmtpClientFactory : ISmtpClientFactory
    {
        public ISmtpClient CreateSmtpClient()
        {
            return new Mock<ISmtpClient>().Object;
        }
    }
}
