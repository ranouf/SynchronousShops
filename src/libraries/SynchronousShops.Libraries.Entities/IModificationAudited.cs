using System;

namespace SynchronousShops.Libraries.Entities
{
    public interface IModificationAudited
    {
        DateTimeOffset? ModifiedAt { get; }
        IModificationAudited Update(DateTimeOffset? modifiedAt);
    }

    public interface IModificationAudited<TUser> : IModificationAudited<Guid?, TUser>
    {
    }

    public interface IModificationAudited<TPrimaryKey, TUser> : IModificationAudited
    {
        TPrimaryKey ModifiedByUserId { get; }
        TUser ModifiedByUser { get; }
        IModificationAudited<TPrimaryKey, TUser> Update(DateTimeOffset? modifiedAt, TUser modifiedByUser);
    }
}
