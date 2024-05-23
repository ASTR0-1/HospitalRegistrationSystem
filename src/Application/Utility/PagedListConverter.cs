using System.Collections.Generic;
using AutoMapper;

namespace HospitalRegistrationSystem.Application.Utility;

/// <summary>
///     Converts a PagedList of one type to a PagedList of another type using AutoMapper.
/// </summary>
/// <typeparam name="TSource">The type of the source items.</typeparam>
/// <typeparam name="TDestination">The type of the destination items.</typeparam>
public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
{
    /// <summary>
    ///     Converts a PagedList of one type to a PagedList of another type.
    /// </summary>
    /// <param name="source">The source PagedList.</param>
    /// <param name="destination">The destination PagedList.</param>
    /// <param name="context">The AutoMapper resolution context.</param>
    /// <returns>The converted PagedList of the destination type.</returns>
    public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
    {
        var destinationItemType = typeof(TDestination);
        var mappedItems = (IEnumerable<TDestination>)context.Mapper.Map(source, source.GetType(), typeof(List<>).MakeGenericType(destinationItemType));

        return new PagedList<TDestination>(
            mappedItems,
            source.MetaData.TotalCount,
            source.MetaData.CurrentPage,
            source.MetaData.PageSize
        );
    }
}
