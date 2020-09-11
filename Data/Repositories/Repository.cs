using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Entities;
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

        public async Task<Page<T>> Query(PagingQuery<T> pagingQuery)
        {
            await using (Context)
            {
                var query = Entities.Where(pagingQuery.Condition);
                var order = pagingQuery.Order;

                order ??= entity => entity.Id;

                query = pagingQuery.IsAscendingOrder ? query.OrderBy(order).ThenBy(entity => entity.Id) : query.OrderByDescending(order).ThenBy(entity => entity.Id);
                query = query.Skip(pagingQuery.PageNumber * pagingQuery.PageSize).Take(pagingQuery.PageSize);

                return Page<T>.From(pagingQuery, await query.ToListAsync());
            }
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