using System;

namespace SynchronousShops.Libraries.Entities
{
    public abstract class AuditedEntity : Entity<Guid>, IEntity, IAudited
    {
        public DateTimeOffset CreatedAt { get; internal set; }
        public DateTimeOffset? ModifiedAt { get; internal set; }
        protected AuditedEntity() { }

        public ICreationAudited Update(DateTimeOffset createdAt)
        {
            CreatedAt = createdAt;
            return this;
        }

        public IModificationAudited Update(DateTimeOffset? modifiedAt)
        {
            ModifiedAt = modifiedAt;
            return this;
        }
    }
}
