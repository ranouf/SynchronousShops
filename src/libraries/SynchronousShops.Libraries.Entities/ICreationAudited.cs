using System;

namespace SynchronousShops.Libraries.Entities
{
    public interface ICreationAudited
    {
        DateTimeOffset CreatedAt { get; }

        ICreationAudited Update(DateTimeOffset createdAt);
    }
    public interface ICreationAudited<TUser> : ICreationAudited<Guid?, TUser>
    {
    }

    public interface ICreationAudited<TPrimaryKey, TUser> : ICreationAudited
    {
        TPrimaryKey CreatedByUserId { get; }
        TUser CreatedByUser { get; }
        ICreationAudited<TPrimaryKey, TUser> Update(DateTimeOffset createdAt, TUser createdByUser);
    }
}
