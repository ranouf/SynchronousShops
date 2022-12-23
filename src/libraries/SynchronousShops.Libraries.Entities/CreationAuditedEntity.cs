using System;

namespace SynchronousShops.Libraries.Entities
{
    public abstract class CreationAuditedEntity : Entity<Guid>, IEntity, ICreationAudited
    {
        public DateTimeOffset CreatedAt { get; internal set; }

        public ICreationAudited Update(DateTimeOffset createdAt)
        {
            CreatedAt = createdAt;
            return this;
        }
    }
}
