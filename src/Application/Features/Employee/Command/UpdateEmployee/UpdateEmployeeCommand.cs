namespace Application.Features.Employee.Command.UpdateEmployee;

public sealed record UpdateEmployeeCommand(Guid Id, EmployeeRequest Request) 
    : ICommand<Result<EmployeeResponse>>;

