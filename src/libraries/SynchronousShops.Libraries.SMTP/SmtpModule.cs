using SynchronousShops.Libraries.SMTP.SmtpClients;
using Autofac;

namespace SynchronousShops.Libraries.SMTP
{
    public class SmtpModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmtpService>().As<ISmtpService>();
            builder.RegisterType<SmtpClientFactory>().As<ISmtpClientFactory>();
        }
    }
}
