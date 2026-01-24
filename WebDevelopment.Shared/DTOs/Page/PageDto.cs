using WebDevelopment.Shared.Enums;

namespace WebDevelopment.Shared.DTOs.Page;

public sealed record PageFilter<T>(
    int PageNumber = 0,
    int PageSize = 10,
    T Filter = default,
    string? SearchKeyword = "",
    SortDirection SortDirection = SortDirection.None,
    string? SortColumn = "");

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