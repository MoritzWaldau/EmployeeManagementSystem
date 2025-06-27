namespace Application.Features.Payroll.Query.GetAllPayrolls;

public sealed class GetAllPayrollsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IQueryHandler<GetAllPayrollsQuery, Result<PaginationResponse<PayrollResponse>>>
{
    public async Task<Result<PaginationResponse<PayrollResponse>>> Handle(GetAllPayrollsQuery request, CancellationToken cancellationToken)
    {
        var totalPayrolls = await unitOfWork.Payrolls.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalPayrolls / request.Request.PageSize);
        var result = await unitOfWork.Payrolls.GetAllAsync(request.Request.PageIndex, request.Request.PageSize, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return Result<PaginationResponse<PayrollResponse>>.Failure(result.ErrorMessage);
        }
        
        var payrolls = result.Value;
        var mappedPayrolls = mapper.Map<List<PayrollResponse>>(payrolls);
        return Result<PaginationResponse<PayrollResponse>>.Success(
            new PaginationResponse<PayrollResponse>(
                request.Request.PageIndex,
                request.Request.PageSize,
                request.Request.PageIndex < totalPages,
                request.Request.PageIndex > 1,
                mappedPayrolls
            )
        );
    }
}