using System;

namespace SynchronousShops.Libraries.Entities
{
    public abstract class FullAuditedEntityOfTUser<TUser> : FullAuditedEntity, IFullAudited<TUser>
        where TUser : IEntity<Guid>
    {
        public Guid? CreatedByUserId { get; internal set; }

        public TUser CreatedByUser { get; internal set; }

        public Guid? ModifiedByUserId { get; internal set; }

        public TUser ModifiedByUser { get; internal set; }

        public Guid? DeletedByUserId { get; internal set; }

        public TUser DeletedByUser { get; internal set; }

        public ICreationAudited<Guid?, TUser> Update(DateTimeOffset createdAt, TUser createdByUser)
        {
            Update(createdAt);
            CreatedByUserId = createdByUser.Id;
            return this;
        }

        public IModificationAudited<Guid?, TUser> Update(DateTimeOffset? modifiedAt, TUser modifiedByUser)
        {
            Update(modifiedAt);
            ModifiedByUserId = modifiedByUser.Id;
            return this;
        }

        IDeletionAudited<Guid?, TUser> IDeletionAudited<Guid?, TUser>.Update(DateTimeOffset? deletedAt, TUser deletedByUser)
        {
            Update(deletedAt);
            DeletedByUserId = deletedByUser.Id;
            return this;
        }
    }
}
