using Autofac;
using System.Reflection;

namespace SynchronousShops.Domains.Core
{
    public class CoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var core = typeof(CoreModule).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(core)
                   .Where(t => t.Name.EndsWith("Manager"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}