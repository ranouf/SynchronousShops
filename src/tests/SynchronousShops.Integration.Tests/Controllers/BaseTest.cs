using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SynchronousShops.Integration.Tests.Data;
using System.Collections.Generic;
using System.Net.Http;
using Xunit.Abstractions;

namespace SynchronousShops.Integration.Tests.Controllers
{
    public class BaseTest
    {
        public ITestOutputHelper Output { get; }
        public TestServer Server { get; }
        public List<string> Logs { get; }
        public HttpClient Client { get; }

        public BaseTest(ITestOutputHelper output)
        {
            Output = output;
            var factory = new TestWebAppFactory<Program>(output);

            // Seed Data
            var serviceProvider = factory.Services;
            using var scope = serviceProvider.CreateScope();
            TestDbInitializer.Seed(scope.ServiceProvider, Output);

            // HttpClient
            Client = factory.CreateClient();

            // Test Server
            Server = factory.Server;

            // Logs
            Logs = factory.Logs;
        }
    }
}
