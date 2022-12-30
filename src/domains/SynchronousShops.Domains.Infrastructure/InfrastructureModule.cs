using Autofac;
using SynchronousShops.Domains.Core.Shops.Etsy;
using SynchronousShops.Domains.Infrastructure.Shops.Etsy;
using SynchronousShops.Domains.Infrastructure.SqlServer;
using SynchronousShops.Libraries.EntityFramework.UnitOfWork;
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

            builder.RegisterType<EtsyShopService>().As<IEtsyShopService>();
            //builder.RegisterType<AllInOneDbContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork<SynchronousShopsDbContext>>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
    }
}
