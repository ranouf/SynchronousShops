using SynchronousShops.Domains.Core.Items;
using SynchronousShops.Domains.Core.Items.Entities;
using SynchronousShops.Domains.Infrastructure.SqlServer;
using Xunit.Abstractions;

namespace SynchronousShops.Integration.Tests.Data
{
    public class TestItemDataBuilder : BaseDataBuilder
    {
        private readonly IItemManager _itemManager;

        public const string Item1 = "Item St Hubert";
        public const string Item2 = "Item St Laurent";
        public const string Item3 = "Item Sherbrooke";

        public TestItemDataBuilder(
            SynchronousShopsDbContext context,
            IItemManager ItemManager,
            ITestOutputHelper output
        ) : base(context, output)
        {
            _itemManager = ItemManager;
        }

        public override void Seed()
        {
            var items = new Item[]
            {
                new Item(Item1),
                new Item(Item2),
                new Item(Item3),
            };

            for (int i = 0; i < items.Length; i++)
            {
                var Item = items[i];
                _itemManager.CreateAsync(Item).Wait();
            }
            Output.WriteLine($"{items.Length} Items have been created.");
        }
    }
}
