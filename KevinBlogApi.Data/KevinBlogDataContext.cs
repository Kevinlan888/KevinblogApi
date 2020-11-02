using KevinBlogApi.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KevinBlogApi.Data
{
    public class KevinBlogDataContext : IdentityDbContext
    {
        public KevinBlogDataContext(DbContextOptions<KevinBlogDataContext> options)
            :base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<Post>();
            modelBuilder.Ignore<IdentityUser>();
            modelBuilder.Ignore<IdentityRole>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Entity<Post>().HasIndex(s => s.Slug).IsUnique();
            modelBuilder.Entity<Post>().ToTable("Posts").HasKey(s => s.PostId);
            modelBuilder.Entity<IdentityUser>().ToTable("Users").HasKey(s => s.Id);
            modelBuilder.Entity<IdentityRole>().ToTable("Roles").HasKey(s => s.Id);
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims").HasKey(s => s.Id);
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles").HasKey(s => s.RoleId);
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin").HasKey(s => s.UserId);
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens").HasKey(s => s.UserId);
        }
    }
}
