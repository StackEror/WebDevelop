using WebDevelopment.Shared.Enums;

namespace WebDevelopment.Shared.Page;

//public sealed record PageFilter<T>(
//    int PageNumber = 0,
//    int PageSize = 10,
//    T Filter = default,
//    string? SearchKeyword = "",
//    SortDirection SortDirection = SortDirection.None,
//    string? SortColumn = "");

public class PageFilter<T>
{
    public int PageNumber { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public T Filter { get; set; }
    public string? SearchKeyword { get; set; } = string.Empty;
    public SortDirection SortDirection { get; set; } = SortDirection.None;
    public string? SortColumn { get; set; } = string.Empty;

    public PageFilter(int pageNumber, int pageSize, T filter, string? searchKeyword, SortDirection sortDirection, string? sortColumn)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Filter = filter;
        SearchKeyword = searchKeyword;
        SortDirection = sortDirection;
        SortColumn = sortColumn;
    }
    public PageFilter()
    {

    }
}
/*
 
 public sealed record PageDto<T>
{
    int PageNumber { get; init; } = 1;
    int PageSize { get; init; } = 10;
    T Filter { get; init; }
    string? SearchKeyword { get; init; } = string.Empty;
    SortDirection SortDirection { get; init; } = SortDirection.None;
    string? SortColumn { get; init; } = string.Empty;

    public PageDto(int pageNumber, int pageSize, T filter, string? searchKeyword, SortDirection sortDirection, string? sortColumn)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Filter = filter;
        SearchKeyword = searchKeyword;
        SortDirection = sortDirection;
        SortColumn = sortColumn;
    }
}

 */