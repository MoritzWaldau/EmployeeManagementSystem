namespace Application.Features.Payroll.Query.GetAllPayrolls;

public sealed class GetAllPayrollsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, HybridCache cache) 
    : IQueryHandler<GetAllPayrollsQuery, Result<PaginationResponse<PayrollResponse>>>
{
    public async Task<Result<PaginationResponse<PayrollResponse>>> Handle(GetAllPayrollsQuery request, CancellationToken cancellationToken)
    {
        var errorMessage = "";
        var cachedValue = await cache.GetOrCreateAsync(
            $"{CacheKeys.PayrollKey}{request.Request.PageIndex}{request.Request.PageSize}",
            async ct =>
            {
                var totalPayrolls = await unitOfWork.Payrolls.CountAsync(ct);
                var totalPages = (int)Math.Ceiling((double)totalPayrolls / request.Request.PageSize);
                var result = await unitOfWork.Payrolls.GetAllAsync(request.Request.PageIndex, request.Request.PageSize, ct);
                
                if (!result.IsSuccess || !result.HasValue)
                {
                    errorMessage = result.ErrorMessage;
                    return null;
                }
                
                var mappedPayrolls = mapper.Map<List<PayrollResponse>>(result.Value);
                
                return new PaginationResponse<PayrollResponse>(
                    request.Request.PageIndex,
                    request.Request.PageSize,
                    request.Request.PageIndex < totalPages,
                    request.Request.PageIndex > 1,
                    mappedPayrolls
                );
            },
            tags: [CacheTags.PayrollTag],
            cancellationToken: cancellationToken
        );

        return cachedValue is null
            ? Result<PaginationResponse<PayrollResponse>>.Failure(errorMessage)
            : Result<PaginationResponse<PayrollResponse>>.Success(cachedValue);
    }
}