using System;

namespace SynchronousShops.Libraries.Entities
{
    public class EntityWithSoftDelete : Entity, ISoftDelete
    {
        public bool IsDeleted { get; internal set; }
        public DateTimeOffset? DeletedAt { get; internal set; }

        public IDeletionAudited Update(DateTimeOffset? deletedAt)
        {
            DeletedAt = deletedAt;
            return this;
        }

        ISoftDelete ISoftDelete.Update(bool isDeleted)
        {
            IsDeleted= isDeleted;
            return this;
        }
    }
}
