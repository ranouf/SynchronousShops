using Microsoft.EntityFrameworkCore;
using System;
using SynchronousShops.Libraries.Entities;

namespace SynchronousShops.Libraries.EntityFramework.Repositories
{
    public class Repository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity>
      where TEntity : class, IEntity<Guid>
    {
        public Repository(DbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
