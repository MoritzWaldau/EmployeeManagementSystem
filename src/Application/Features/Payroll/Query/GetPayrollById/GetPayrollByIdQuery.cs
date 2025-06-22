namespace Application.Features.Payroll.Query.GetPayrollById;

public sealed record GetPayrollByIdQuery(Guid Id) : IQuery<GetPayrollByIdResponse>;

public sealed record GetPayrollByIdResponse(PayrollResponse Response);