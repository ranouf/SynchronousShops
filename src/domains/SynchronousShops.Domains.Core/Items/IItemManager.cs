using SynchronousShops.Domains.Core.Items.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynchronousShops.Domains.Core.Items
{
    public interface IItemManager
    {
        Task<Item> CreateAsync(Item item);
        Task DeleteAsync(Item item);
        Task<Item> FindByIdAsync(Guid id);
        Task<IList<Item>> GetAllAsync(string filter);
        Task<Item> UpdateAsync(Item item);
    }
}