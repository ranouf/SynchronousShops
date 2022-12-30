using Autofac;

namespace SynchronousShops.Domains.Core.Session
{
    public class NullSessionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NullSession>().As<IUserSession>().SingleInstance();
        }
    }
}