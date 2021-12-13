using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace MVVM_firstApp.Models
{
    public class GenericRepository<T> where T: class
    {
        private readonly LotoContext dbContext;
        private readonly DbSet<T> dbSet;

        public GenericRepository(LotoContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }

        public virtual T Add(T entity)
        {
            return this.dbSet.Add(entity).Entity;
        }

        public virtual T Delete(T entity)
        {
            return this.dbSet.Remove(entity).Entity;
        }

        //public virtual T DeleteAll()
        //{
        //    Z.EntityFramework.Plus
        //    this.dbSet.Delete();
        //}

        //public virtual int Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateFactory)
        //{
        //    Z.EntityFramework.Plus
        //    return this.dbSet.Where(predicate).Update(Update);
        //} 

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return this.dbSet.Where(predicate).AsEnumerable();
        }

        public IEnumerable<T> GetAll()
        {
            return this.dbSet.ToList();
        }
        public T GetById(int id)
        {
            return this.dbSet.Find(id);
        }

        public IEnumerable<T> GetWithInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] paths)
        {
            IQueryable<T> queryable = this.dbSet.Where(predicate);
            //if(paths != null)
            //{
            //    Z.EntityFramework.Plus.QueryIncludeOptimized.EF6
            //    queryable = paths.Aggregate(queryable, (current, path) => current.IncludeOptimized(path));
            //}

            return queryable.AsEnumerable();
        }
        public virtual void Save()
        {
            this.dbContext.SaveChanges();
        }
    }
}
