namespace Application.Models.Pagination;

public sealed record PaginationRequest(int PageIndex = 1, int PageSize = 10)
{
    [JsonPropertyName("pageIndex")]
    public int PageIndex { get; } = PageIndex < 1 ? 1 : PageIndex;
    
    [JsonPropertyName("pageSize")]
    public int PageSize { get; } = PageSize < 1 ? 10 : PageSize;
};