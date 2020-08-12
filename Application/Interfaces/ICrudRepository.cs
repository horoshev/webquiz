using System.Collections.Generic;

namespace Application.Interfaces
{
    /// <summary>
    /// CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface ICrudRepository<T> where T : class
    {
        /// <summary>
        /// Fetch all records from database
        /// </summary>
        /// <returns>Collection of records</returns>
        IEnumerable<T> GetAll();
        T Get(int entityId);
        T Create(T entity);
        void Update(T entity);
        void Delete(T entity);

        /// <summary>
        /// Delete record from database using identifier
        /// </summary>
        /// <param name="entityId">unique identifier</param>
        void Delete(int entityId);
    }
}