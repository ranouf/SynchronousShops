using SynchronousShops.Libraries.SMTP.SmtpClients;
using Autofac;

namespace SynchronousShops.Libraries.SMTP
{
    public class TestSmtpModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmtpService>().As<ISmtpService>();
            builder.RegisterType<TestSmtpClientFactory>().As<ISmtpClientFactory>();
        }
    }
}
