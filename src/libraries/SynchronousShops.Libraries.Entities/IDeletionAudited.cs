using System;

namespace SynchronousShops.Libraries.Entities
{
    public interface IDeletionAudited
    {
        DateTimeOffset? DeletedAt { get; }
        IDeletionAudited Update(DateTimeOffset? deletedAt);
    }

    public interface IDeletionAudited<TUser> : IDeletionAudited<Guid?, TUser>
    {
    }

    public interface IDeletionAudited<TPrimaryKey, TUser> : IDeletionAudited
    {
        TPrimaryKey DeletedByUserId { get; }
        TUser DeletedByUser { get; }
        IDeletionAudited<TPrimaryKey, TUser> Update(DateTimeOffset? deletedAt, TUser deletedByUser);
    }
}
