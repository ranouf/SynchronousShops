using SynchronousShops.Servers.API.Controllers.Dtos;
using System;

namespace SynchronousShops.Servers.API.Controllers.Identity.Dtos
{
    public class RoleDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
