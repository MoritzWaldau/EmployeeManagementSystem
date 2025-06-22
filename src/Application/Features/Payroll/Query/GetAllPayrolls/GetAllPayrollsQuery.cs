using Application.Models.Pagination;

namespace Application.Features.Payroll.Query.GetAllPayrolls;

public sealed record GetAllPayrollsQuery(PaginationRequest Request) : IQuery<GetAllPayrollsQueryResult>;

public sealed record GetAllPayrollsQueryResult(PaginationResponse<PayrollResponse> Response);