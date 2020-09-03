using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Create new or get common repository of specified entity type.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <returns>Common repository of specified entity type.</returns>
        IRepository<T> GetRepository<T>() where T : class, IRepositoryEntity;

        /// <summary>
        /// Create new or get base repository of specified entity type.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <returns>Base repository of specified entity type.</returns>
        IBaseRepository<T> GetBaseRepository<T>() where T : class, IRepositoryEntity;

        /// <summary>
        /// Commit changes.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Async implementation of 'SaveChanges' with CancellationToken.
        /// </summary>
        /// <returns>Nothing.</returns>
        Task SaveChangesAsync();

        /// <summary>
        /// Async implementation of 'SaveChanges' with CancellationToken.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Nothing.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}