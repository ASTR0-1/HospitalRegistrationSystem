using System;
using System.Linq;
using System.Linq.Expressions;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
///     Base repository class for CRUD operations.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly ApplicationContext _applicationContext;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RepositoryBase{T}" /> class.
    /// </summary>
    /// <param name="applicationContext">The application context.</param>
    public RepositoryBase(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    /// <summary>
    ///     Creates a new entity.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    public void Create(T entity) => _applicationContext.Set<T>().Add(entity);

    /// <summary>
    ///     Deletes an entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public void Delete(T entity) => _applicationContext.Set<T>().Remove(entity);

    /// <summary>
    ///     Updates an entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public void Update(T entity) => _applicationContext.Set<T>().Update(entity);

    /// <summary>
    ///     Finds all entities.
    /// </summary>
    /// <param name="trackChanges">Flag indicating whether to track changes.</param>
    /// <returns>The queryable collection of entities.</returns>
    public IQueryable<T> FindAll(bool trackChanges)
    {
        return !trackChanges
            ? _applicationContext.Set<T>()
                .AsNoTracking()
            : _applicationContext.Set<T>();
    }

    /// <summary>
    ///     Finds entities based on a condition.
    /// </summary>
    /// <param name="expression">The condition expression.</param>
    /// <param name="trackChanges">Flag indicating whether to track changes.</param>
    /// <returns>The queryable collection of entities.</returns>
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
    {
        return !trackChanges
            ? _applicationContext.Set<T>()
                .Where(expression)
                .AsNoTracking()
            : _applicationContext.Set<T>()
                .Where(expression);
    }
}
