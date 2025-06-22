namespace Application.Models.Pagination;

public sealed record PaginationRequest(int PageIndex = 1, int PageSize = 10)
{
    public int PageIndex { get; } = PageIndex < 1 ? 1 : PageIndex;
    public int PageSize { get; } = PageSize < 1 ? 10 : PageSize;
};