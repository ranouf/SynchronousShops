using Autofac;
using SynchronousShops.Libraries.EntityFramework.Repositories;

namespace SynchronousShops.Libraries.EntityFramework
{
    public class EntityFrameworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>));
        }
    }
}
