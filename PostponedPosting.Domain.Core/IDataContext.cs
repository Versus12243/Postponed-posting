using PostponedPosting.Domain.Entities;
using System.Collections.Generic;
using System.Data.Entity;

namespace PostponedPosting.Domain.Core
{
    public interface IDataContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
    }
}
