using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class Repository<T> : CrudRepository<T>, IRepository<T> where T : class
    {
        protected Repository(DbContext context) : base(context)
        {
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return Entities.Where(predicate).ToList();
        }

        public T GetByIndex(int entityIndex)
        {
            return Entities.Skip(entityIndex).FirstOrDefault();
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        public int Count()
        {
            return Entities.Count();
        }
    }
}