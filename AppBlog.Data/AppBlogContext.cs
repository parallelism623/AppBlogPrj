using AppBlog.Core.Domain;
using AppBlog.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBlog.Data
{
    public class AppBlogContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppBlogContext(DbContextOptions options) : base(options)
        {

        }
        #region DbSet
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostActivityLog> PostActivityLogs { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<PostInSeries> PostInSeries { get; set; }

        #endregion DbSet

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims").HasKey(x => x.Id);
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new {x.RoleId, x.UserId});
            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => new { x.UserId });
        }
        //public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        //{
        //    var entity = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added);
        //    foreach(var entities in entity)
        //    {
        //        var datePro = entities.Entity.GetType().GetProperty("DateCreated");
        //        if (entities.State == EntityState.Added && datePro != null)
        //        {
        //            datePro.SetValue(entities.Entity, DateTime.Now);
        //        }
        //        var dateMo = entities.Entity.GetType().GetProperty("ModifiedDate");
        //        if (entities.State == EntityState.Modified && dateMo != null)
        //        {
        //            dateMo.SetValue(entities.Entity, DateTime.Now);
        //        }
        //    }
        //    return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //}
    }
}
