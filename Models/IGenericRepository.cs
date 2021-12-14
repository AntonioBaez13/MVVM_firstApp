using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace MVVM_firstApp.Models
{
    public interface IGenericRepository<T> where T : class
    {
        T Add(T entity);

        T Delete(T entity);

        void Delete(Expression<Func<T, bool>> predicate);

        int Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateFactory);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        T GetById(int id);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetWithInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] paths);
        void Save();
    }
}
