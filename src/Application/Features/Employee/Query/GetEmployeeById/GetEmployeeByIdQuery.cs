namespace Application.Features.Employee.Query.GetEmployeeById;

public sealed record GetEmployeeByIdQuery(Guid Id) : IQuery<Result<EmployeeResponse>>;
