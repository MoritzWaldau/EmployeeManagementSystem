using Application.Features.Employee.Query.GetAllEmployees;
using Application.Models.Pagination;

namespace Application.Features.Payroll.Query.GetAllPayrolls;

public sealed class GetAllPayrollsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IQueryHandler<GetAllPayrollsQuery, GetAllPayrollsQueryResult>
{
    public async Task<GetAllPayrollsQueryResult> Handle(GetAllPayrollsQuery request, CancellationToken cancellationToken)
    {
        var totalPayrolls = await unitOfWork.Payrolls.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalPayrolls / request.Request.PageSize);
        var payrolls = await unitOfWork.Payrolls.GetAllAsync(request.Request.PageIndex, request.Request.PageSize, cancellationToken);
        var mappedPayrolls = mapper.Map<List<PayrollResponse>>(payrolls);
        return new GetAllPayrollsQueryResult(
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