using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    /// <summary>
    /// Additional operations for repository
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T> : IDisposable where T : class, IRepositoryEntity
    {
        void IDisposable.Dispose()
        {
            SaveChanges();
        }

        /// <summary>
        /// Select questions with specified query.
        /// </summary>
        /// <param name="predicate">Expression used for querying questions.</param>
        /// <returns>Collection of matched questions.</returns>
        IEnumerable<T> Query(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Returning question in specified position.
        /// </summary>
        /// <param name="entityIndex">Position of the question in collection.</param>
        /// <returns>Founded question or null.</returns>
        T? GetByIndex(int entityIndex);

        /// <summary>
        /// Commit changes.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Returns number of questions in collection.
        /// </summary>
        /// <returns>Number of questions in collection.</returns>
        int Count();
    }
}