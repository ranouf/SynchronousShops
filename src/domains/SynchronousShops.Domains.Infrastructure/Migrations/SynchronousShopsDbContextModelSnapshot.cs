﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SynchronousShops.Domains.Infrastructure.SqlServer;

#nullable disable

namespace SynchronousShops.Domains.Infrastructure.Migrations
{
    [DbContext(typeof(SynchronousShopsDbContext))]
    partial class SynchronousShopsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("SynchronousShops.Domains.Core.Identity.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("SynchronousShops.Domains.Core.Identity.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeletedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("nvarchar(max)")
                        .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

                    b.Property<Guid?>("InvitedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("ModifiedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("DeletedByUserId");

                    b.HasIndex("Email");

                    b.HasIndex("InvitedByUserId");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("SynchronousShops.Domains.Core.Identity.Entities.UserMetadata", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Key")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Key");

                    b.HasIndex("UserId");

                    b.ToTable("UserMetadata");
                });

            modelBuilder.Entity("SynchronousShops.Domains.Core.Identity.Entities.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("SynchronousShops.Domains.Core.Items.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeletedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("ModifiedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("DeletedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("Name");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SynchronousShops.Domains.Core.Identity.Entities.User", b =>
                {
                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.User", "DeletedByUser")
                        .WithMany()
                        .HasForeignKey("DeletedByUserId");

                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.User", "InvitedByUser")
                        .WithMany()
                        .HasForeignKey("InvitedByUserId");

                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.Navigation("CreatedByUser");

                    b.Navigation("DeletedByUser");

                    b.Navigation("InvitedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("SynchronousShops.Domains.Core.Identity.Entities.UserMetadata", b =>
                {
                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.User", "User")
                        .WithMany("UserMetadata")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SynchronousShops.Domains.Core.Identity.Entities.UserRole", b =>
                {
                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SynchronousShops.Domains.Core.Items.Entities.Item", b =>
                {
                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.User", "DeletedByUser")
                        .WithMany()
                        .HasForeignKey("DeletedByUserId");

                    b.HasOne("SynchronousShops.Domains.Core.Identity.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.Navigation("CreatedByUser");

                    b.Navigation("DeletedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("SynchronousShops.Domains.Core.Identity.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("SynchronousShops.Domains.Core.Identity.Entities.User", b =>
                {
                    b.Navigation("UserMetadata");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
