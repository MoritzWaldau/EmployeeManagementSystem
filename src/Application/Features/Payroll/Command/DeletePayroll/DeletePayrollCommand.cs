namespace Application.Features.Payroll.Command.DeletePayroll;

public sealed record DeletePayrollCommand(Guid Id) : ICommand;