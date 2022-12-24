using Autofac;
using Microsoft.Extensions.Logging;
using SynchronousShops.Libraries.SMTP;

namespace SynchronousShops.Integration.Tests
{
    public class TestsModule : Module
    {
        private readonly ILogger<TestsModule> _logger;

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<TestSmtpModule>();
        }
    }
}
