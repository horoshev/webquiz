using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IRepositoryEntity
    {
        protected readonly DbContext Context;
        protected readonly DbSet<T> Entities;

        public Repository(DbContext context)
        {
            Context = context;
            Entities = Context.Set<T>();
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate)
        {
            using (Context)
            {
                return Entities.Where(predicate).ToList();
            }
        }

        public T GetByIndex(int entityIndex)
        {
            return Entities.Skip(entityIndex).FirstOrDefault();
        }

        public void SaveChanges()
        {
            Context?.SaveChanges();
        }

        // TODO: Delete unused
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