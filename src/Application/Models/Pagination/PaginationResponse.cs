namespace Application.Models.Pagination;

public sealed record PaginationResponse<TEntity>(
    [property: JsonPropertyName("pageIndex")]
    int PageIndex, 
    [property: JsonPropertyName("pageSize")]
    int PageSize, 
    [property: JsonPropertyName("totalCount")]
    bool HasNextPage, 
    [property: JsonPropertyName("hasNextPage")]
    bool HasPreviousPage, 
    [property: JsonPropertyName("items")]
    IEnumerable<TEntity> Items)
    where TEntity : class;
