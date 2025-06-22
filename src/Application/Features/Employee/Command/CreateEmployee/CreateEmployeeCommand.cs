namespace Application.Features.Employee.Command.CreateEmployee;

public sealed record CreateEmployeeCommand(EmployeeRequest Request) : ICommand<CreateEmployeeCommandResult>;

public sealed record CreateEmployeeCommandResult(EmployeeResponse Response);