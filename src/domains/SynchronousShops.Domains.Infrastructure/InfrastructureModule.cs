using Autofac;
using SynchronousShops.Libraries.EntityFramework.UnitOfWork;
using SynchronousShops.Domains.Infrastructure.SQLServer;
using System.Reflection;

namespace SynchronousShops.Domains.Infrastructure
{
    public class InfrastructureModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder.RegisterType<UnitOfWork<SKSQLDbContext>>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
    }
}