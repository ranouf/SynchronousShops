namespace SynchronousShops.Libraries.SMTP.SmtpClients
{
    public interface ISmtpClientFactory
    {
        ISmtpClient CreateSmtpClient();
    }
}