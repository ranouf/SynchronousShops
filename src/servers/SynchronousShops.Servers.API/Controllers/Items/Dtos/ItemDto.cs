using SynchronousShops.Servers.API.Controllers.Dtos.Entities;

namespace SynchronousShops.Servers.API.Controllers.Items.Dtos
{
    public class ItemDto : EntityDto, IEntityDto
    {
        public string Name { get; set; }
    }
}
