namespace Application.Features.Payroll.Query.GetAllPayrolls;

public sealed record GetAllPayrollsQuery(PaginationRequest Request) : IQuery<Result<PaginationResponse<PayrollResponse>>>;