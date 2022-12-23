using SynchronousShops.Libraries.Entities;

namespace SynchronousShops.Domains.Core.Items.Entities
{
    public class Item : Entity, IEntity
    {
        public string Name { get; private set; }
        public Item(string name)
        {
            Name = name;
        }

        public Item Update(string name)
        {
            if (Name != name) Name = name;
            return this;
        }

    }
}
