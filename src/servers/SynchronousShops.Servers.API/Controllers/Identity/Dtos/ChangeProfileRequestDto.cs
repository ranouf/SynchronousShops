using SynchronousShops.Servers.API.Controllers.Dtos;
using System.ComponentModel.DataAnnotations;

namespace SynchronousShops.Servers.API.Controllers.Identity.Dtos
{
    public class ChangeProfileRequestDto : IDto
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
    }
}
