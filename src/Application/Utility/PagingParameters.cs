namespace HospitalRegistrationSystem.Application.Utility;

/// <summary>
///     Represents the paging parameters for retrieving data.
/// </summary>
public class PagingParameters
{
    private const int MaxPageSize = 50;

    private int _pageSize = 10;

    /// <summary>
    ///     Gets or sets the page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    ///     Gets or sets the page size.
    /// </summary>
    /// <remarks>
    ///     The page size is limited to a maximum value of 50.
    /// </remarks>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize 
            ? MaxPageSize
            : value;
    }
}
