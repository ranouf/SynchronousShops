using Autofac;
using Microsoft.AspNetCore.Http;
using SynchronousShops.Domains.Core;
using SynchronousShops.Domains.Infrastructure;
using SynchronousShops.Libraries.Authentication;
using SynchronousShops.Libraries.Session;
using SynchronousShops.Libraries.Settings;
using SynchronousShops.Servers.API.Filters;
using SynchronousShops.Servers.API.SignalR.Connection;

namespace SynchronousShops.Servers.API
{
    public class APIModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
            builder.RegisterType<ApiExceptionFilter>();
            builder.RegisterType<SettingsValidator>();

            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<AuthenticationModule>();
            builder.RegisterModule<HttpContextSessionModule>();

            // SignalR
            builder.RegisterType<ConnectionService>().As<IConnectionService>().SingleInstance();
        }
    }
}
