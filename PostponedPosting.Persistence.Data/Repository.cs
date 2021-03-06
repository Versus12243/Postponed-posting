﻿using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.Data
{
        public class Repository<T> : IRepository<T> where T : class
        {
            [Inject]
            public IDataContext _context { get; set; }
            private IDbSet<T> _entities;

            public Repository(IDataContext context)
            {
                this._context = context;
            }

            public T GetById(object id)
            {
                return this.Entities.Find(id);
            }

            public T Find(Expression<Func<T, bool>> predicate)
            {
                return this.Entities.FirstOrDefault(predicate);
            }

            public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate)
            {
                return this.Entities.Where(predicate);
            }

        public void Insert(T entity)
            {
                try
                {
                    if (entity == null)
                    {
                        throw new ArgumentNullException("entity");
                    }
                    this.Entities.Add(entity);
                    this._context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    var msg = string.Empty;

                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                        }
                    }

                    var fail = new Exception(msg, dbEx);
                    throw fail;
                }
            }

        public async Task InsertAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entities.Add(entity);
                await this._context.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Update(T entity)
            {
                try
                {
                    if (entity == null)
                    {
                        throw new ArgumentNullException("entity");
                    }
                    this._context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    var msg = string.Empty;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    var fail = new Exception(msg, dbEx);
                    throw fail;
                }
            }

            public void Delete(T entity)
            {
                try
                {
                    if (entity == null)
                    {
                        throw new ArgumentNullException("entity");
                    }
                    this.Entities.Remove(entity);
                    this._context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    var msg = string.Empty;

                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    var fail = new Exception(msg, dbEx);
                    throw fail;
                }
            }

            public virtual IQueryable<T> Table
            {
                get
                {
                    return this.Entities;
                }
            }

            private IDbSet<T> Entities
            {
                get
                {
                    if (_entities == null)
                    {
                        _entities = _context.Set<T>();
                    }
                    return _entities;
                }
            }

            public IList<T> GetAll()
            {
                return this.Entities.ToList();
            }
    }
}
