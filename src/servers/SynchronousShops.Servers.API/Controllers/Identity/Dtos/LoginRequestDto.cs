using SynchronousShops.Servers.API.Controllers.Dtos;
using System.ComponentModel.DataAnnotations;

namespace SynchronousShops.Servers.API.Controllers.Identity.Dtos
{
    public class LoginRequestDto : IDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
