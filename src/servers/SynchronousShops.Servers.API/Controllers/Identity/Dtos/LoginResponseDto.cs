using SynchronousShops.Servers.API.Controllers.Dtos;

namespace SynchronousShops.Servers.API.Controllers.Identity.Dtos
{
    public class LoginResponseDto : IDto
    {
        public string Token { get; set; }
        public UserDto CurrentUser { get; set; }
    }
}
