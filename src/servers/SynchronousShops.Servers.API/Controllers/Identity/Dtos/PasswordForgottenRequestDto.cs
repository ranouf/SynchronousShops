using SynchronousShops.Servers.API.Controllers.Dtos;
using System.ComponentModel.DataAnnotations;

namespace SynchronousShops.Servers.API.Controllers.Identity.Dtos
{
    public class PasswordForgottenRequestDto : IDto
    {
        [Required]
        public string Email { get; set; }
    }
}
