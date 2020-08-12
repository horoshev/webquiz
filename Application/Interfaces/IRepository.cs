using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IRepository<T> : ICrudRepository<T> where T : class
    {
        IEnumerable<T> Query(Expression<Func<T, bool>> predicate);
        T? GetByIndex(int entityIndex);
        void SaveChanges();
        int Count();
    }
}