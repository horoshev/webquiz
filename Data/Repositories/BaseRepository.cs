using System.Collections.Generic;
using System.Linq;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class BaseRepository<T> : Repository<T>, IBaseRepository<T> where T : class, IRepositoryEntity
    {
        public BaseRepository(DbContext context) : base(context)
        {
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

        public T Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public T Delete(T entity)
        {
            return Entities.Remove(entity).Entity;
        }

        public T Delete(int entityId)
        {
            var entity = Entities.Find(entityId);

            return Entities.Remove(entity).Entity;
        }
    }
}