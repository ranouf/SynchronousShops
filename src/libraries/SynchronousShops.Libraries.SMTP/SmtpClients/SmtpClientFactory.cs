namespace SynchronousShops.Libraries.SMTP.SmtpClients
{
    public class SmtpClientFactory : ISmtpClientFactory
    {
        public ISmtpClient CreateSmtpClient()
        {
            return new SmtpClient();
        }
    }
}
