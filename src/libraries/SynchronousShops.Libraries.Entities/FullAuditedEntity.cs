using System;

namespace SynchronousShops.Libraries.Entities
{
    public abstract class FullAuditedEntity : AuditedEntity, IFullAudited
    {
        public bool IsDeleted { get; internal set; }

        public DateTimeOffset? DeletedAt { get; internal set; }

        public IDeletionAudited Update(bool isDeleted)
        {
            IsDeleted = isDeleted;
            return this;
        }

        IDeletionAudited IDeletionAudited.Update(DateTimeOffset? deletedAt)
        {
            DeletedAt = deletedAt;
            return this;
        }
    }
}
