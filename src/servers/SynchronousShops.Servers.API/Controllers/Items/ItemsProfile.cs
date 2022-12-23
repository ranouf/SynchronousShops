using AutoMapper;
using SynchronousShops.Domains.Core.Items.Entities;
using SynchronousShops.Servers.API.Controllers.Items.Dtos;

namespace SynchronousShops.Servers.API.Controllers.Items
{
    public class ItemsProfile : Profile
    {
        public ItemsProfile()
        {
            CreateMap<Item, ItemDto>();
        }
    }
}
