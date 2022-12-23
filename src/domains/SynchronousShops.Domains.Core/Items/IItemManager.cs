using SynchronousShops.Domains.Core.Items.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynchronousShops.Domains.Core.Items
{
    public interface IItemManager
    {
        Task<Item> CreateItemAsync(Item item);
        Task DeleteItemAsync(Item item);
        Task<Item> FindByIdAsync(Guid id);
        Task<IList<Item>> GetItems(string filter);
        Task<Item> UpdateItemAsync(Item item);
    }
}