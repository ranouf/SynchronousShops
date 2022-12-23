using System;

namespace SynchronousShops.Libraries.Entities
{
    public interface IAudited : ICreationAudited, IModificationAudited
    {
    }
    public interface IAudited<TUser> : ICreationAudited<TUser>, IModificationAudited<TUser>
         where TUser : IEntity<Guid>
    {
    }
}
