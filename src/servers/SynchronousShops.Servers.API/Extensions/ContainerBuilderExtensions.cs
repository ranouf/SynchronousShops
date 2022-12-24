using Autofac;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Linq;
using System.Reflection;

namespace SynchronousShops.Servers.API.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void Register(this ContainerBuilder containerBuilder, Type type)
        {
            var assemblies = DependencyContext.Default.RuntimeLibraries
                .Where(library => library.Name.StartsWith(nameof(SynchronousShops)))
                .Select(library => Assembly.Load(new AssemblyName(library.Name)))
                .ToList();

            containerBuilder.RegisterAssemblyTypes(assemblies.ToArray())
                .AsClosedTypesOf(type)
                .InstancePerLifetimeScope();
        }
    }
}
