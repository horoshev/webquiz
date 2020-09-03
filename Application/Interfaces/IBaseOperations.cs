using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IBaseOperations<in TIn, out TOut> where TOut : class
    {
        /// <summary>
        /// Returns all entities in data source.
        /// </summary>
        /// <returns>All entities in data source.</returns>
        IEnumerable<TOut> GetAll();

        /// <summary>
        /// Returns entity with provided identifier.
        /// </summary>
        /// <param name="entityId">Identifier of the entity.</param>
        /// <returns>Founded entity or null if entity was not found.</returns>
        TOut? Get(int entityId);

        /// <summary>
        /// Create new entity record in data source.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>Created entity or null if entity was not created.</returns>
        TOut? Create(TIn entity);

        /// <summary>
        /// Update entity in data source.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>Updated entity or null if entity was not found.</returns>
        TOut? Update(TIn entity);

        /// <summary>
        /// Delete entity from data source.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <returns>Deleted entity or null if entity was not found.</returns>
        TOut? Delete(TIn entity);

        /// <summary>
        /// Delete entity from data source by identifier.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <returns>Deleted entity or null if entity was not found.</returns>
        TOut? Delete(int entityId);
    }
}