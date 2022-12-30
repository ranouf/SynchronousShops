using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling.Internal;
using SynchronousShops.Domains.Core.Identity;
using SynchronousShops.Domains.Core.Items;
using SynchronousShops.Domains.Core.Items.Entities;
using SynchronousShops.Domains.Core.Session;
using SynchronousShops.Libraries.Constants;
using SynchronousShops.Servers.API.Controllers.Dtos;
using SynchronousShops.Servers.API.Controllers.Items.Dtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SynchronousShops.Servers.API.Controllers.Items
{
    [ApiController]
    [Route(Api.V1.Item.Url)]
    public class ItemController : AuthentifiedBaseController
    {
        private readonly IItemManager _itemManager;

        public ItemController(
            IItemManager itemManager,
            IUserSession session,
            IUserManager userManager,
            IMapper mapper,
            ILogger<ItemController> logger
        ) : base(session, userManager, mapper, logger)
        {
            _itemManager = itemManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ItemDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetItemByIdAsync([FromRoute] Guid id)
        {
            Logger.LogInformation($"{nameof(GetItemByIdAsync)}, id:{id}.");
            var result = await _itemManager.FindByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return new ObjectResult(Mapper.Map<Item, ItemDto>(result));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<ItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetItemsAsync([FromQuery] FilterRequestDto dto)
        {
            Logger.LogInformation($"{nameof(GetItemsAsync)}, dto:{dto.ToJson()}.");
            var result = await _itemManager.GetAllAsync(
                dto.Filter
            );
            return new ObjectResult(Mapper.Map<IList<Item>, IList<ItemDto>>(result));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateItemAsync([FromBody] UpsertItemRequest dto)
        {
            Logger.LogInformation($"{nameof(CreateItemAsync)}, dto:{dto.ToJson()}.");
            var result = await _itemManager.CreateAsync(
                dto.ToItem()
            );
            return new ObjectResult(Mapper.Map<Item, ItemDto>(result));
        }

        [HttpPut]
        [ProducesResponseType(typeof(ItemDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateItemAsync([FromRoute] Guid id, [FromBody] UpsertItemRequest dto)
        {
            Logger.LogInformation($"{nameof(UpdateItemAsync)}, id:{id}, dto:{dto.ToJson()}.");
            var item = await _itemManager.FindByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            var result = await _itemManager.UpdateAsync(
                item.Update(
                    dto.Name
                )
            );
            return new ObjectResult(Mapper.Map<Item, ItemDto>(result));
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteItemAsync([FromRoute] Guid id)
        {
            Logger.LogInformation($"{nameof(UpdateItemAsync)}, id:{id}.");
            var item = await _itemManager.FindByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _itemManager.DeleteAsync(
                item
            );
            return Ok();
        }
    }
}