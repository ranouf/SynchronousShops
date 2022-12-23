using System;

namespace SynchronousShops.Libraries.Entities
{
    public abstract class AuditedEntityOfTUser<TUser> : AuditedEntity, IAudited<TUser>
        where TUser : IEntity<Guid>
    {
        public Guid? CreatedByUserId { get; internal set; }
        public TUser CreatedByUser { get; internal set; }
        public Guid? ModifiedByUserId { get; internal set; }
        public TUser ModifiedByUser { get; internal set; }
        protected AuditedEntityOfTUser() { }

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
    }
}
