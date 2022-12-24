using SynchronousShops.Domains.Core.Identity.Entities;
using SynchronousShops.Servers.API.Controllers.Identity.Dtos;
using SynchronousShops.Servers.API.Extensions;
using AutoMapper;

namespace SynchronousShops.Servers.API.Controllers.Identity
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<User, UserDto>()
                .AddFullAuditedBy()
                .ForMember(
                    dest => dest.InvitedBy,
                    opts => opts.MapFrom(src => src.InvitedByUser.FullName)
                );
            CreateMap<Role, RoleDto>();
        }
    }
}
