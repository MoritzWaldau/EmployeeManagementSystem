namespace Application.Features.Payroll.Command.UpdatePayroll;

public sealed record UpdatePayrollCommand(Guid Id, PayrollRequest Request) : ICommand<Result<PayrollResponse>>;

