using System.Collections.Generic;
using System.Linq;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CrudRepository<T> : ICrudRepository<T> where T : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<T> Entities;

        protected CrudRepository(DbContext context)
        {
            Context = context;
            Entities = Context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.ToList();
        }

        public T Get(int entityId)
        {
            return Entities.Find(entityId);
        }

        public T Create(T entity)
        {
            Entities.Add(entity);

            return entity;
        }

        public void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            Entities.Remove(entity);
        }

        public void Delete(int entityId)
        {
            var entity = Entities.Find(entityId);
            Entities.Remove(entity);
        }
    }
}
