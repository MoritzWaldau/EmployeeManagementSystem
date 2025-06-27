namespace Application.Features.Payroll.Command.CreatePayroll;

public sealed record CreatePayrollCommand(PayrollRequest PayrollRequest) : ICommand<Result<PayrollResponse>>;
