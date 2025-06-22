namespace Application.Features.Payroll.Command.UpdatePayroll;

public sealed record UpdatePayrollCommand(Guid Id, PayrollRequest Request) : ICommand<UpdatePayrollCommandResponse>;

public sealed record UpdatePayrollCommandResponse(PayrollResponse Response);