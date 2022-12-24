using Microsoft.AspNetCore.Identity;
using SynchronousShops.Libraries.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SynchronousShops.Domains.Core.Identity.Entities
{
    public class User : IdentityUser<Guid>, IEntity, IAudited<User>, ISoftDelete<User>
    {
        [Required]
        public string Firstname { get; internal set; }
        [Required]
        public string Lastname { get; internal set; }

        private string _fullName;
        public string FullName
        {
            get
            {
                return _fullName ?? $"{Firstname} {Lastname}";
            }
            private set
            {
                _fullName = value;
            }
        }
        public Guid? InvitedByUserId { get; internal set; }
        public virtual User InvitedByUser { get; internal set; }

        [NotMapped]
        public string RoleName
        {
            get
            {
                return UserRoles?.FirstOrDefault()?.Role?.Name;
            }
        }
        public ICollection<UserRole> UserRoles { get; internal set; } = new List<UserRole>();

        public DateTimeOffset CreatedAt { get; internal set; }
        public Guid? CreatedByUserId { get; internal set; }
        public virtual User CreatedByUser { get; internal set; }
        public DateTimeOffset? ModifiedAt { get; internal set; }
        public Guid? ModifiedByUserId { get; internal set; }
        public virtual User ModifiedByUser { get; internal set; }
        public bool IsDeleted { get; internal set; }
        public DateTimeOffset? DeletedAt { get; internal set; }
        public Guid? DeletedByUserId { get; internal set; }
        public virtual User DeletedByUser { get; internal set; }

        internal User()
        {
            GenerateNewSecurityStamp();
        }

        public User(
            string userName,
            string firstname,
            string lastname,
            User invitedByUser = null,
            bool emailConfirmed = false
        ) : base(userName)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = userName;
            if (invitedByUser != null) InvitedByUserId = invitedByUser.Id;
            EmailConfirmed = emailConfirmed;
        }

        public void Update(string firstname, string lastname)
        {
            Firstname = firstname;
            Lastname = lastname;
        }

        public bool Equals(IEntity x, IEntity y)
        {
            return x.Id == y.Id;
        }

        public void GenerateNewSecurityStamp()
        {
            SecurityStamp = Guid.NewGuid().ToString();
        }

        public User SetRole(Role role)
        {
            if (RoleName != role.Name)
            {
                UserRoles.Clear();
                UserRoles.Add(new UserRole()
                {
                    User = this,
                    Role = role
                });
            }
            return this;
        }

        public ICreationAudited<Guid?, User> Update(DateTimeOffset createdAt, User createdByUser)
        {
            Update(createdAt);
            CreatedByUserId = createdByUser.Id;
            return this;
        }

        public ICreationAudited Update(DateTimeOffset createdAt)
        {
            CreatedAt = createdAt;
            return this;
        }

        public IModificationAudited<Guid?, User> Update(DateTimeOffset? modifiedAt, User modifiedByUser)
        {
            Update(modifiedAt);
            ModifiedByUserId = modifiedByUser.Id;
            return this;
        }

        public IModificationAudited Update(DateTimeOffset? modifiedAt)
        {
            ModifiedAt = modifiedAt;
            return this;
        }

        public ISoftDelete<Guid?, User> Update(bool isDeleted)
        {
            IsDeleted = isDeleted;
            return this;
        }

        IDeletionAudited<Guid?, User> IDeletionAudited<Guid?, User>.Update(DateTimeOffset? deletedAt, User deletedByUser)
        {
            Update(deletedAt);
            DeletedByUserId = deletedByUser.Id;
            return this;
        }

        IDeletionAudited IDeletionAudited.Update(DateTimeOffset? deletedAt)
        {
            DeletedAt = deletedAt;
            return this;
        }
    }
}
