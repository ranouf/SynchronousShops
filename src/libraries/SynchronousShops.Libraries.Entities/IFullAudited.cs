using System;

namespace SynchronousShops.Libraries.Entities
{
    public interface IFullAudited : IAudited, IDeletionAudited
    {
    }
    public interface IFullAudited<TUser> : IAudited<TUser>, IDeletionAudited<TUser>
         where TUser : IEntity<Guid>
    {
    }
}
