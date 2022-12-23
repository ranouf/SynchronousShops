using Autofac;
using Autofac.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SynchronousShops.Libraries.Extensions;
using SynchronousShops.Libraries.Extensions;
using SynchronousShops.Libraries.Settings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SynchronousShops.Libraries.Settings
{
    public static class AzureSettingsGenerator
    {
        public static string Generate(IServiceProvider services)
        {
            var types = services
                .GetService<ILifetimeScope>()
                .ComponentRegistry
                .Registrations
                .SelectMany(e => e.Services)
                .Select(s => s as TypedService)
                .Where(s => s.ServiceType.IsAssignableToGenericType(typeof(IConfigureOptions<>)))
                .Select(s => s.ServiceType.GetGenericArguments()[0])
                .Where(s => s.Name.EndsWith("Settings"))
                .ToList();

            var settings = new List<object>();
            foreach (var t in types)
            {
                var option = services.GetService(typeof(IOptions<>).MakeGenericType(new Type[] { t })).GetPropertyValue("Value");
                settings.AddRange(option.ToAzureSettings(t.Name.Replace("Settings", "")));

            }
            return settings.ToJson();
        }
    }
}
