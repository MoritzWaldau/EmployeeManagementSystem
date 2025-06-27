namespace Application.Features.Payroll.Query.GetPayrollById;

public sealed record GetPayrollByIdQuery(Guid Id) : IQuery<Result<PayrollResponse>>;