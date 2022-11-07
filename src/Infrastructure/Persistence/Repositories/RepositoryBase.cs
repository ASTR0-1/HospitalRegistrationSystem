using System;
using System.Linq;
using System.Linq.Expressions;
using HospitalRegistrationSystem.Application.Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryContext _repositoryContext;

    public RepositoryBase(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }

    public void Create(T entity) =>
        _repositoryContext.Set<T>().Add(entity);

    public void Delete(T entity) =>
        _repositoryContext.Set<T>().Remove(entity);

    public void Update(T entity) =>
        _repositoryContext.Set<T>().Update(entity);

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
            _repositoryContext.Set<T>()
                .AsNoTracking() :
            _repositoryContext.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges ?
            _repositoryContext.Set<T>()
                .Where(expression)
                .AsNoTracking() :
            _repositoryContext.Set<T>()
                .Where(expression);
}
