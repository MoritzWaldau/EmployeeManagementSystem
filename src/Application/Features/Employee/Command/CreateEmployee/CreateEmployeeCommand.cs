namespace Application.Features.Employee.Command.CreateEmployee;

public sealed record CreateEmployeeCommand(EmployeeRequest Employee) : ICommand<Result<EmployeeResponse>>;
