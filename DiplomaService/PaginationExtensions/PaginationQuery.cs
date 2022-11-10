namespace DiplomaService.PaginationExtensions;

public class PaginationQuery
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    
    /// <summary>
    /// Page number
    /// </summary>
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Amount of data you need on single page. Must be greater than 0.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}