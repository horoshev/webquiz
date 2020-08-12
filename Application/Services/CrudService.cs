using System.Collections.Generic;
using Application.Interfaces;

namespace Application.Services
{
    public class CrudService<T> : ICrudRepository<T> where T : class
    {
        private readonly IRepository<T> _repository;

        protected CrudService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T Get(int entityId)
        {
            return _repository.Get(entityId);
        }

        public T Create(T entity)
        {
            _repository.Create(entity);
            _repository.SaveChanges();

            return entity;
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
            _repository.SaveChanges();
        }

        public void Delete(T entity)
        {
            _repository.Delete(entity);
            _repository.SaveChanges();
        }

        public void Delete(int entityId)
        {
            _repository.Delete(entityId);
            _repository.SaveChanges();
        }
    }
}