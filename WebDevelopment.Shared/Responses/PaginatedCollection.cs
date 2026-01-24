namespace WebDevelopment.Shared.Responses;

public record PaginatedCollection<T>(
    int TotalRecords = 0,
    int TotalPages = 0,
    int PageNumber = 0,
    int PageSize = 0,
    ICollection<T> Pages = default
);