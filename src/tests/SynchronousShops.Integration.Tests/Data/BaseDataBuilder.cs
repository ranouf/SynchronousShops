using SynchronousShops.Domains.Infrastructure.SQLServer;
using Xunit.Abstractions;

namespace SynchronousShops.Integration.Tests.Data
{
    public abstract class BaseDataBuilder
    {
        public readonly SKSQLDbContext Context;
        public readonly ITestOutputHelper Output;

        public BaseDataBuilder(SKSQLDbContext context, ITestOutputHelper output)
        {
            Context = context;
            Output = output;
        }

        public abstract void Seed();
    }
}
