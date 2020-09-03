using System.Collections.Generic;
using Application.Interfaces;
using AutoMapper;

namespace Application.Services
{
    public class BaseService<TIn, TOut> : IBaseOperations<TIn, TOut>
        where TIn : IRepositoryEntity
        where TOut : class, IRepositoryEntity
    {
        protected readonly IBaseRepository<TOut> Repository;
        protected readonly IMapper Mapper;

        protected BaseService(IUnitOfWork container, IMapper mapper)
        {
            Repository = container.GetBaseRepository<TOut>();
            Mapper = mapper;
        }

        public IEnumerable<TOut> GetAll()
        {
            return Repository.GetAll();
        }

        public TOut? Get(int entityId)
        {
            return Repository.Get(entityId);
        }

        public TOut? Create(TIn entity)
        {
            if (entity is null)
            {
                return null;
            }

            var created = Repository.Create(Mapper.Map<TOut>(entity));
            Repository.SaveChanges();

            return created;
        }

        public TOut? Update(TIn entity)
        {
            return entity is null ? null : Repository.Update(Mapper.Map<TOut>(entity));
        }

        public TOut? Delete(TIn entity)
        {
            return entity is null ? null : Delete(entity.Id);
        }

        public TOut? Delete(int entityId)
        {
            return Repository.Delete(entityId);
        }
    }
}