using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HospitalRegistrationSystem.WebAPI.ActionFilters;

/// <summary>
///     Represents a filter that adds pagination headers to the HTTP response.
/// </summary>
public class PaginationHeaderFilter : IAsyncResultFilter
{
    /// <summary>
    ///     Adds pagination headers to the HTTP response.
    /// </summary>
    /// <param name="context">The result executing context.</param>
    /// <param name="next">The delegate representing the next result execution.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var result = context.Result as ObjectResult;

        if (result?.Value is IPagedList pagedList)
        {
            context.HttpContext
                .Response
                .Headers
                .Append("X-Pagination", JsonConvert.SerializeObject(pagedList.MetaData, Formatting.None));
        }

        await next();
    }
}
