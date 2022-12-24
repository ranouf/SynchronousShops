using Autofac;

namespace SynchronousShops.Libraries.Session
{
    public class HttpContextSessionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextSession>().As<IUserSession>();
        }
    }
}
