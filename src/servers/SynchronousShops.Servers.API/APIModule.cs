using Autofac;
using SynchronousShops.Domains.Core;
using SynchronousShops.Domains.Infrastructure;
using SynchronousShops.Libraries.Settings;

namespace SynchronousShops.Servers.API
{
    public class APIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SettingsValidator>();

            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<InfrastructureModule>();
        }
    }
}