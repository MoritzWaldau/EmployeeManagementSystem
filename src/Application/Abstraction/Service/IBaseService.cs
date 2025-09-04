

namespace Application.Abstraction.Service;

public interface IBaseService<in TRequest, TResponse> where TResponse : BaseResponse
{
    Task<Result<PaginationResponse<TResponse>>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
    Task<Result<TResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<TResponse>> CreateAsync(TRequest request, CancellationToken cancellationToken = default);
    Task<Result<TResponse>> UpdateAsync(Guid id, TRequest request, CancellationToken cancellationToken = default);
    Task<Result<TResponse>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}