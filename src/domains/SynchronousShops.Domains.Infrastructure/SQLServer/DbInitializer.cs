using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SynchronousShops.Domains.Infrastructure.SQLServer
{
    public static class DbInitializer
    {
        public async static Task InitializeAsync(IServiceProvider services, ILogger logger)
        {
            await services.MigrateDatabaseAsync(logger);
        }

        #region Private
        private static async Task MigrateDatabaseAsync(this IServiceProvider services, ILogger logger)
        {
            try
            {
                logger.LogInformation("Start database migration.");
                var context = services.GetRequiredService<SKSQLDbContext>();
                if (context.Database.IsRelational())
                {
                    await context.Database.MigrateAsync();
                }
                logger.LogInformation("Database migration has been done.");
            }
            catch (Exception ex)
            {
                logger.LogError("Database migration has failed.", ex);
                throw;
            }
        }
        #endregion
    }
}
