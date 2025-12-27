using WebDevelopment.Shared.Enums;

namespace WebDevelopment.Shared.RequestFeatures;

public class DataRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string Sort { get; set; } = string.Empty;
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    public string? Keyword { get; set; } = string.Empty;
    public List<string> SearchableColumns { get; set; }
    public object? Filter { get; set; }
    public DataRequest() 
    {
    }
    public DataRequest(DataRequest request)
    {
        Page = request.Page;
        PageSize = request.PageSize;
        Sort = request.Sort;
        SortDirection = request.SortDirection;
        Keyword = request.Keyword;
        SearchableColumns = request.SearchableColumns;
        Filter = request.Filter;
    }
}
