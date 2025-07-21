namespace Shared.Models.Pagination;

public sealed record PaginationResponse<TEntity>(
    int PageIndex, 
    int PageSize, 
    bool HasNextPage, 
    bool HasPreviousPage, 
    IEnumerable<TEntity> Items
) where TEntity : BaseResponse;
