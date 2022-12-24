using SynchronousShops.Servers.API.Controllers.Dtos;
using SynchronousShops.Servers.API.Controllers.Dtos.Entities;
using System;

namespace SynchronousShops.Servers.API.Controllers.Identity.Dtos
{
    public class UserDto : EntityDto<Guid?>, IEntityDto<Guid?>, IAuditedDto, IDeletionAuditedDto, IComparable, IDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ProfileImageUrl { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool IsLocked { get; set; }
        public string RoleName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public string DeletedBy { get; set; }
        public string InvitedBy { get; set; }

        public int CompareTo(object obj)
        {
            if (!(obj is UserDto user))
            {
                throw new InvalidCastException($"Not able to cast as '{typeof(UserDto).Name}'.");
            }
            return FullName.CompareTo(user.FullName);
        }
    }
}
