using Autofac;

namespace SynchronousShops.Libraries.Authentication
{
    public class AuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JWTAuthenticationService>().As<IAuthenticationService>();
        }
    }
}
