using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SynchronousShops.Libraries.Settings;
using System;
using System.Threading.Tasks;

namespace SynchronousShops.Servers.Operations
{
    public class Program
    {
        public static async Task Main()
        {
            var host = CreateHostBuilder().Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            ValidateSettings(services, logger);

            using (host)
            {
                await host.RunAsync();
            }
        }

        static void ValidateSettings(IServiceProvider services, ILogger<Program> logger)
        {
            try
            {
                logger.LogInformation("Starting settings validation.");
                services.GetRequiredService<SettingsValidator>();
                logger.LogInformation("The settings has been validated.");
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred while validating the settings.", ex);
            }
        }

        static IHostBuilder CreateHostBuilder() => new HostBuilder()
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;

                //Settings
                //services.ConfigureAndValidate<QueueSettings>(configuration);
            })
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule<MessagerModule>();
            })
            .ConfigureLogging((hostContext, configLogging) =>
            {
                if (hostContext.HostingEnvironment.IsDevelopment())
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                }
            })
            .UseConsoleLifetime();
    }
}
