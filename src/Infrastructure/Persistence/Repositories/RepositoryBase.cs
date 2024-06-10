using System;
using System.Linq;
using System.Linq.Expressions;
using HospitalRegistrationSystem.Application.Interfaces.Data;
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

    /// <inheritdoc />
    public void Create(T entity)
    {
        _applicationContext.Set<T>().Add(entity);
    }

    /// <inheritdoc />
    public void Delete(T entity)
    {
        _applicationContext.Set<T>().Remove(entity);
    }

    /// <inheritdoc />
    public void Update(T entity)
    {
        _applicationContext.Set<T>().Update(entity);
    }

    /// <inheritdoc />
    public IQueryable<T> FindAll(bool trackChanges)
    {
        return !trackChanges
            ? _applicationContext.Set<T>()
                .AsNoTracking()
            : _applicationContext.Set<T>();
    }

    /// <inheritdoc />
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _applicationContext.Set<T>();

        if (!trackChanges)
            query = query.AsNoTracking();

        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return query.Where(expression);
    }
}