namespace Application.Interfaces
{
    public interface IBaseRepository<T> : IRepository<T>, IBaseOperations<T, T> where T : class, IRepositoryEntity
    {
    }
}