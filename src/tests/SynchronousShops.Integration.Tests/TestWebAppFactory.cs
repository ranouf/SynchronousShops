using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SynchronousShops.Domains.Infrastructure.SQLServer;
using SynchronousShops.Libraries.Testing.Logging.InMemory;
using SynchronousShops.Libraries.Testing.Logging.Xunit;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit.Abstractions;

namespace SynchronousShops.Integration.Tests
{
    public class TestWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
    {
        public ITestOutputHelper Output { get; set; }
        public List<string> Logs { get; private set; } = new List<string>();

        public TestWebAppFactory([NotNull] ITestOutputHelper output)
        {
            Output = output;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<SKSQLDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<SKSQLDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("SK.Sample");
                    });

                    //var sp = services.BuildServiceProvider();
                    //using (var scope = sp.CreateScope())
                    //using (var appContext = scope.ServiceProvider.GetRequiredService<SKDbContext>())
                    //{
                    //    try
                    //    {
                    //        appContext.Database.EnsureCreated();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        //Log errors or do anything you think it's needed
                    //        throw;
                    //    }
                    //}WebApplicationFactory
                })
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddXunitLogger(Output);
                    builder.AddInMemoryLogger(Logs);
                })
                ;

        }
    }
}