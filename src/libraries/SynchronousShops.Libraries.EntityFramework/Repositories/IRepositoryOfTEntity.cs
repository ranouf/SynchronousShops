using SynchronousShops.Libraries.Entities;
using System;

namespace SynchronousShops.Libraries.EntityFramework.Repositories
{
    /// <summary>
    /// A shortcut of <see cref="IRepository{TEntity,TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : class, IEntity<Guid>
    {

    }
}
