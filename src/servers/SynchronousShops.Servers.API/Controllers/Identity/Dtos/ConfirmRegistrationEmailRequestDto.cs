using SynchronousShops.Servers.API.Controllers.Dtos;

namespace SynchronousShops.Servers.API.Controllers.Identity.Dtos
{
    public class ConfirmRegistrationEmailRequestDto : IDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
