using Microsoft.Extensions.DependencyInjection;
using SynchronousShops.Domains.Core.Identity;
using SynchronousShops.Domains.Core.Items;
using SynchronousShops.Domains.Infrastructure.SqlServer;
using System;
using Xunit.Abstractions;

namespace SynchronousShops.Integration.Tests.Data
{
    public static class TestDbInitializer
    {
        public static void Seed(IServiceProvider services, ITestOutputHelper output)
        {
            try
            {
                var context = services.GetRequiredService<SynchronousShopsDbContext>();
                if (context.Database.EnsureCreated())
                {
                    var itemManager = services.GetRequiredService<IItemManager>();

                    new TestItemDataBuilder(context, itemManager, output).Seed();

                    var userManager = services.GetRequiredService<IUserManager>();
                    var roleManager = services.GetRequiredService<IRoleManager>();

                    new TestUserDataBuilder(context, userManager, roleManager, output).Seed();

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                output.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
