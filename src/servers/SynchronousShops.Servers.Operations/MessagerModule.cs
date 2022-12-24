using Autofac;
using SynchronousShops.Libraries.Settings;

namespace SynchronousShops.Servers.Operations
{
    public class MessagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SettingsValidator>();
        }
    }
}
