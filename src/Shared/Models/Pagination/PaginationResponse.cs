namespace Shared.Models.Pagination;

public sealed record PaginationResponse<TEntity>(
    int PageIndex, 
    int PageSize, 
    bool HasNextPage, 
    bool HasPreviousPage,
    int PageCount,
    IEnumerable<TEntity> Items
) where TEntity : BaseResponse;
