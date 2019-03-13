using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OrderApi.Data
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        T Add(T entity);
        IEnumerable<T> Search(Expression<Func<T, bool>> predicate);
        void Edit(T entity);
        void Remove(int id);
    }
}
