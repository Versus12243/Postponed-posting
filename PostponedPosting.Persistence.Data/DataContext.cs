using PostponedPosting.Domain.Core;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using PostponedPosting.Domain.Entities;
using PostponedPosting.Domain.Entities.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PostponedPosting.Domain.Entities.SocialNetworkModel;
using PostponedPosting.Entities.CredentialModel;
using PostponedPosting.Domain.Entities.PostModels;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>, IDataContext
    {
        public DataContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<SocialNetwork> SocialNetworks { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PagesGroup> PagesGroups { get; set; }

        public static DataContext Create()
        {
            return new DataContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasMany(p => p.Posts).WithRequired(u => u.User).WillCascadeOnDelete(true);
            modelBuilder.Entity<SocialNetwork>().HasMany(p => p.Posts).WithRequired(s => s.SocialNetwork).WillCascadeOnDelete(true);

            modelBuilder.Entity<Post>().HasRequired(u => u.User);
            modelBuilder.Entity<Post>().HasRequired(s => s.SocialNetwork);

            base.OnModelCreating(modelBuilder);
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}
