using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SynchronousShops.Domains.Core.Identity.Entities;
using SynchronousShops.Domains.Core.Items.Entities;
using SynchronousShops.Libraries.Entities;
using SynchronousShops.Libraries.Extensions;
using SynchronousShops.Libraries.Session;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronousShops.Domains.Infrastructure.SqlServer
{
    public class SynchronousShopsDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public IUserSession _session { get; private set; }
        protected Guid? UserId { get; set; }

        public DbSet<Item> Items { get; set; }

        public SynchronousShopsDbContext() { }

        public SynchronousShopsDbContext(DbContextOptions<SynchronousShopsDbContext> options) : base(options) { }

        public SynchronousShopsDbContext(
            DbContextOptions<SynchronousShopsDbContext> options,
            IUserSession session
        ) : base(options)
        {
            _session = session;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            PrepareIdentityModel(builder);
            PrepareItemModel(builder);

            static void PrepareIdentityModel(ModelBuilder builder)
            {
                builder.Entity<User>(entity =>
                {
                    entity.HasOne(e => e.CreatedByUser)
                        .WithMany()
                        .HasForeignKey(e => e.CreatedByUserId);
                    entity.HasOne(e => e.ModifiedByUser)
                        .WithMany()
                        .HasForeignKey(e => e.ModifiedByUserId);
                    entity.HasOne(e => e.DeletedByUser)
                        .WithMany()
                        .HasForeignKey(e => e.DeletedByUserId);
                    entity.HasOne(e => e.InvitedByUser)
                        .WithMany()
                        .HasForeignKey(e => e.InvitedByUserId);
                    entity.HasIndex(e => e.Email);
                    entity.HasIndex(e => e.NormalizedEmail);
                    entity.HasIndex(e => e.IsDeleted);
                    entity.Property(e => e.FullName)
                        .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");
                    entity.HasIndex(e => e.IsDeleted);
                    entity.HasQueryFilter(e => !e.IsDeleted);
                });

                builder.Entity<Role>(entity =>
                {
                    entity.HasIndex(e => e.Name);
                    entity.HasIndex(e => e.NormalizedName);
                });

                builder.Entity<UserRole>(entity =>
                {
                    entity.HasKey(e => new { e.UserId, e.RoleId });

                    entity.HasOne(e => e.Role)
                        .WithMany(e => e.UserRoles)
                        .HasForeignKey(e => e.RoleId)
                        .IsRequired();

                    entity.HasOne(e => e.User)
                        .WithMany(e => e.UserRoles)
                        .HasForeignKey(e => e.UserId)
                        .IsRequired();
                });
            }

            static void PrepareItemModel(ModelBuilder builder)
            {
                builder.Entity<Item>(entity =>
                {
                    entity.HasIndex(e => e.Name);
                    entity.HasQueryFilter(e => !e.IsDeleted);
                });
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_session.UserId.HasValue)
            {
                var user = Users.FirstOrDefault(e => e.Id == _session.UserId.Value);
                if (user != null)
                {
                    UserId = user.Id;
                }
            }

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

                        if (entry.Entity.IsAssignableToGenericType(typeof(ISoftDelete<>))
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
            if (entry.Entity.IsAssignableToGenericType(typeof(ICreationAudited<>)))
            {
                if (entry.Entity.TryGetPropertyValue("CreatedAt", out DateTimeOffset? createdAt) && !createdAt.HasValue)
                {
                    entry.Entity.SetPropertyValue<DateTimeOffset?>("CreatedAt", DateTime.Now);
                }

                if (entry.Entity.TryGetPropertyValue("CreatedByUserId", out Guid? createdByUserId) && !createdByUserId.HasValue)
                {
                    entry.Entity.SetPropertyValue("CreatedByUserId", UserId);
                }
            }
        }

        protected virtual void SetModificationAuditProperties(EntityEntry entry)
        {
            if (entry.Entity.IsAssignableToGenericType(typeof(IModificationAudited<>)))
            {
                entry.Entity.SetPropertyValue<DateTimeOffset?>("UpdatedAt", DateTime.Now);
                entry.Entity.SetPropertyValue("UpdatedByUserId", UserId);
            }
        }

        protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (entry.Entity.IsAssignableToGenericType(typeof(ISoftDelete<>)))
            {
                entry.State = EntityState.Unchanged;
                entry.Entity.SetPropertyValue("IsDeleted", true);
            }
        }

        protected virtual void SetDeletionAuditProperties(EntityEntry entry)
        {
            if (entry.Entity.IsAssignableToGenericType(typeof(IDeletionAudited<>)))
            {
                if (entry.Entity.TryGetPropertyValue("DeletedAt", out DateTimeOffset? deletedAt) && !deletedAt.HasValue)
                {
                    entry.Entity.SetPropertyValue<DateTimeOffset?>("DeletedAt", DateTime.Now);
                }

                if (entry.Entity.TryGetPropertyValue("DeletedByUserId", out Guid? deletedByUserId) && !deletedByUserId.HasValue)
                {
                    entry.Entity.SetPropertyValue("DeletedByUserId", UserId);
                }
            }
        }
    }
}
