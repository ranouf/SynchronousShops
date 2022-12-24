using SynchronousShops.Servers.API.Controllers.Dtos.Entities;
using System;

namespace SynchronousShops.Servers.API.Controllers.Items.Dtos
{
    public class ItemDto : EntityDto, IEntityDto, IAuditedDto
    {
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
