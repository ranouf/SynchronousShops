using SynchronousShops.Domains.Infrastructure.SqlServer;
using Xunit.Abstractions;

namespace SynchronousShops.Integration.Tests.Data
{
    public abstract class BaseDataBuilder
    {
        public readonly SynchronousShopsDbContext Context;
        public readonly ITestOutputHelper Output;

        public BaseDataBuilder(SynchronousShopsDbContext context, ITestOutputHelper output)
        {
            Context = context;
            Output = output;
        }

        public abstract void Seed();
    }
}
