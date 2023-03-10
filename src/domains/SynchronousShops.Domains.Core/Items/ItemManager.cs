using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SynchronousShops.Domains.Core.Items.Entities;
using SynchronousShops.Libraries.EntityFramework.Repositories;
using SynchronousShops.Libraries.EntityFramework.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynchronousShops.Domains.Core.Items
{
    public class ItemManager : IItemManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Item> _itemRepository;
        private readonly ILogger<ItemManager> _logger;

        public ItemManager(
            IUnitOfWork unitOfWork,
            ILogger<ItemManager> logger
        )
        {
            _unitOfWork = unitOfWork;
            _itemRepository = unitOfWork.GetRepository<Item>();
            _logger = logger;
        }

        public async Task<IList<Item>> GetAllAsync(string filter)
        {
            var query = _itemRepository.GetAll();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(e => e.Name.Contains(filter));
            }
            var result = await query.ToListAsync();
            _logger.LogInformation($"Got {result.Count} items returned.", result);
            return result;
        }

        public async Task<Item> FindByIdAsync(Guid id)
        {
            var result = await _itemRepository.GetAll()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
            if (result == null)
            {
                _logger.LogInformation($"No item found for id:'{id}'");
            }
            else
            {
                _logger.LogInformation($"Item found for id:'{id}'", result);
            }
            return result;
        }

        public async Task<Item> CreateAsync(Item item)
        {
            var result = _itemRepository.Insert(item);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation($"Item with id:{result.Id}' has been created.", result);
            return result;
        }

        public async Task<Item> UpdateAsync(Item item)
        {
            var result = _itemRepository.Update(item);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation($"Item with id:{result.Id}' has been updated.", result);
            return result;
        }

        public async Task DeleteAsync(Item item)
        {
            _itemRepository.Delete(item);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation($"Item with id:{item.Id}' has been deleted.", item);
        }
    }
}
