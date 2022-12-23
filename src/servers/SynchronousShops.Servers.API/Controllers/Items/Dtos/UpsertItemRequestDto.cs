using SynchronousShops.Domains.Core.Items.Entities;
using SynchronousShops.Servers.API.Controllers.Dtos;
using System.ComponentModel.DataAnnotations;

namespace SynchronousShops.Servers.API.Controllers.Items.Dtos
{
    public class UpsertItemRequest : IDto
    {
        [Required]
        public string Name { get; set; }
        public Item ToItem()
        {
            return new Item(Name);
        }
    }
}
