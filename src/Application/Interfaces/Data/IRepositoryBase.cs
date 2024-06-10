using System;
using System.Linq;
using System.Linq.Expressions;

namespace HospitalRegistrationSystem.Application.Interfaces.Data;

/// <summary>
///     Represents a base repository interface for CRUD operations.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public interface IRepositoryBase<T> where T : class
{
    /// <summary>
    ///     Retrieves all entities.
    /// </summary>
    /// <param name="trackChanges">Indicates whether to track changes in the entities.</param>
    /// <returns>An <see cref="IQueryable{T}" /> representing the collection of entities.</returns>
    IQueryable<T> FindAll(bool trackChanges);

    /// <summary>
    ///     Retrieves entities based on the specified condition.
    /// </summary>
    /// <param name="expression">The condition to filter the entities.</param>
    /// <param name="trackChanges">Indicates whether to track changes in the entities.</param>
    /// <param name="includeProperties">The properties to include in the query results.</param>
    /// <returns>An <see cref="IQueryable{T}" /> representing the filtered collection of entities.</returns>
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges,
        params Expression<Func<T, object>>[] includeProperties);

    /// <summary>
    ///     Creates a new entity.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    void Create(T entity);

    /// <summary>
    ///     Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    ///     Deletes an entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(T entity);
}