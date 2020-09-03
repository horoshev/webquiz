using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly Dictionary<Type, IRepository<IRepositoryEntity>> _repositories;

        public UnitOfWork(WebQuizDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, IRepository<IRepositoryEntity>>();
        }

        public IRepository<T> GetRepository<T>() where T : class, IRepositoryEntity
        {
            if (_repositories.TryGetValue(typeof(T), out var repository))
            {
                return repository as IRepository<T>;
            }

            var newRepository = new Repository<T>(_dbContext);
            _repositories.Add(typeof(T), newRepository as IRepository<IRepositoryEntity>);

            return newRepository;
        }

        public IBaseRepository<T> GetBaseRepository<T>() where T : class, IRepositoryEntity
        {
            if (_repositories.TryGetValue(typeof(T), out var repository))
            {
                return repository as IBaseRepository<T>;
            }

            var baseRepository = new BaseRepository<T>(_dbContext);
            _repositories.Add(typeof(T), baseRepository as IRepository<IRepositoryEntity>);

            return baseRepository;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext?.SaveChanges();
        }
    }
}