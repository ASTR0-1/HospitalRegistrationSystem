using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalRegistrationSystem.Application.Utility.PagedData;

/// <inheritdoc cref="IPagedList" />
/// <typeparam name="T">The type of items in the paged list.</typeparam>
public class PagedList<T> : List<T>, IPagedList
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PagedList{T}" /> class.
    /// </summary>
    /// <param name="items">The items in the paged list.</param>
    /// <param name="count">The total count of items.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The page size.</param>
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData
        {
            TotalCount = count,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int) Math.Ceiling(count / (double) pageSize)
        };

        AddRange(items);
    }

    /// <inheritdoc />
    public MetaData MetaData { get; }

    /// <summary>
    ///     Creates a paged list from the source collection.
    /// </summary>
    /// <param name="source">The source collection.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The paged list.</returns>
    public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }

    /// <summary>
    ///     Creates a paged list asynchronously from the source queryable.
    /// </summary>
    /// <param name="source">The source queryable.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The paged list.</returns>
    public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}