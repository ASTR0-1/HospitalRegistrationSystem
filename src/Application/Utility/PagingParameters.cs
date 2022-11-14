namespace HospitalRegistrationSystem.Application.Utility;

public class PagingParameters
{
    private const int _maxPageSize = 10;
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 4;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
        }
    }
}
