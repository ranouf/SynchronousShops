using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SynchronousShops.Libraries.Entities;
using SynchronousShops.Libraries.Extensions;
using SynchronousShops.Domains.Core.Items.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronousShops.Domains.Infrastructure.SQLServer
{
    public class SKSQLDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public SKSQLDbContext() { }

        public SKSQLDbContext(DbContextOptions<SKSQLDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            PrepareItemModel(builder);

            static void PrepareItemModel(ModelBuilder builder)
            {
                builder.Entity<Item>(entity =>
                {
                    entity.HasIndex(u => u.Name);
                });
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        SetCreationAuditProperties(entry);
                        break;
                    case EntityState.Modified:
                        SetModificationAuditProperties(entry);

                        if (entry.Entity.IsAssignableToGenericType(typeof(ISoftDelete))
                            && entry.Entity.TryGetPropertyValue("IsDeleted", out bool isDeleted)
                            && isDeleted
                        )
                        {
                            SetDeletionAuditProperties(entry);
                        }
                        break;
                    case EntityState.Deleted:
                        CancelDeletionForSoftDelete(entry);
                        SetDeletionAuditProperties(entry);
                        break;
                }
            }
        }

        protected virtual void SetCreationAuditProperties(EntityEntry entry)
        {
            if (entry.Entity.IsAssignableToGenericType(typeof(ICreationAudited)))
            {
                if (entry.Entity.TryGetPropertyValue("CreatedAt", out DateTimeOffset? createdAt) && !createdAt.HasValue)
                {
                    entry.Entity.SetPropertyValue<DateTimeOffset?>("CreatedAt", DateTime.Now);
                }
            }
        }

        protected virtual void SetModificationAuditProperties(EntityEntry entry)
        {
            if (entry.Entity.IsAssignableToGenericType(typeof(IModificationAudited)))
            {
                entry.Entity.SetPropertyValue<DateTimeOffset?>("UpdatedAt", DateTime.Now);
            }
        }

        protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (entry.Entity.IsAssignableToGenericType(typeof(ISoftDelete)))
            {
                entry.State = EntityState.Unchanged;
                entry.Entity.SetPropertyValue("IsDeleted", true);
            }
        }

        protected virtual void SetDeletionAuditProperties(EntityEntry entry)
        {
            if (entry.Entity.IsAssignableToGenericType(typeof(IDeletionAudited)))
            {
                if (entry.Entity.TryGetPropertyValue("DeletedAt", out DateTimeOffset? deletedAt) && !deletedAt.HasValue)
                {
                    entry.Entity.SetPropertyValue<DateTimeOffset?>("DeletedAt", DateTime.Now);
                }
            }
        }
    }
}
