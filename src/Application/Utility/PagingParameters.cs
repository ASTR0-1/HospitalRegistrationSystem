namespace HospitalRegistrationSystem.Application.Utility;

public class PagingParameters
{
    private const int _maxPageSize = 10;

    private int _pageSize = 4;
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > _maxPageSize ? _maxPageSize : value;
    }
}