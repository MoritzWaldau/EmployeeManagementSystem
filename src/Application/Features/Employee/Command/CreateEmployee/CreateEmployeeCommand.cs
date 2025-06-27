namespace Application.Features.Employee.Command.CreateEmployee;

public sealed record CreateEmployeeCommand(EmployeeRequest Request) : ICommand<Result<EmployeeResponse>>;
