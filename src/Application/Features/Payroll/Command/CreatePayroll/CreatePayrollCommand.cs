using Application.Models.Payroll;

namespace Application.Features.Payroll.Command.CreatePayroll;

public sealed record CreatePayrollCommand(PayrollRequest PayrollRequest) : ICommand<CreatePayrollResponse>;

public sealed record CreatePayrollResponse(PayrollResponse Response);