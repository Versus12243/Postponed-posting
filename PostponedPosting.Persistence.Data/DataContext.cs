using PostponedPosting.Domain.Core;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using PostponedPosting.Domain.Entities;
using PostponedPosting.Domain.Entities.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PostponedPosting.Domain.Entities.SocialNetworkModel;
using PostponedPosting.Entities.CredentialModel;

namespace PostponedPosting.Persistence.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>, IDataContext
    {
        public DataContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Social_network> Social_networks { get; set; }
        public DbSet<Access_token> Access_tokens { get; set; }

        public static DataContext Create()
        {
            return new DataContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {          
            base.OnModelCreating(modelBuilder);
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}
