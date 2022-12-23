using System;

namespace SynchronousShops.Libraries.Entities
{
    public abstract class CreationAuditedEntityOfTUser<TUser> : CreationAuditedEntity, ICreationAudited<TUser>
        where TUser : IEntity
    {
        public Guid? CreatedByUserId { get; internal set; }

        public TUser CreatedByUser { get; internal set; }

        public ICreationAudited<Guid?, TUser> Update(DateTimeOffset createdAt, TUser createdByUser)
        {
            Update(createdAt);
            CreatedByUserId = createdByUser.Id; ;
            return this;
        }
    }
}
