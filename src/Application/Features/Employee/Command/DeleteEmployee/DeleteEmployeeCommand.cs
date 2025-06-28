namespace Application.Features.Employee.Command.DeleteEmployee;

public sealed record DeleteEmployeeCommand(Guid Id) : ICommand<Result<EmployeeResponse>>;