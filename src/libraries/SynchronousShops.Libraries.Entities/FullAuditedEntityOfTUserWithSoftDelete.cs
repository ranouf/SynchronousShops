using System;

namespace SynchronousShops.Libraries.Entities
{
    public abstract class FullAuditedEntityOfTUserWithSoftDelete<TUser> : FullAuditedEntityOfTUser<TUser>, ISoftDelete<TUser>
        where TUser : IEntity
    {
        ISoftDelete<Guid?, TUser> ISoftDelete<Guid?, TUser>.Update(bool isDeleted)
        {
            Update(isDeleted);
            return this;
        }
    }
}
