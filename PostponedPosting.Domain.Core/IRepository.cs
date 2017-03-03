using PostponedPosting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Domain.Core
{
        public interface IRepository<T>: IDisposable where T : class
        {
            T GetById(object id);            
            void Insert(T entity);
            Task InsertAsync(T entity);
            void Update(T entity);
            void Delete(T entity);
            IQueryable<T> Table { get; }
            T Find(Expression<Func<T, bool>> predicate);
            IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);
            IList<T> GetAll();
        }    
}
