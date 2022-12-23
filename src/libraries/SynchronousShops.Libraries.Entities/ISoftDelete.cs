using System;

namespace SynchronousShops.Libraries.Entities
{
    public interface ISoftDelete : IDeletionAudited
    {
        bool IsDeleted { get; }
        ISoftDelete Update(bool isDeleted);
    }

    public interface ISoftDelete<TUser> : ISoftDelete<Guid?, TUser>, IDeletionAudited<TUser>
    {
    }

    public interface ISoftDelete<TPrimaryKey, TUser> : IDeletionAudited<TPrimaryKey, TUser>
    {
        bool IsDeleted { get; }
        ISoftDelete<TPrimaryKey, TUser> Update(bool isDeleted);
    }
}
