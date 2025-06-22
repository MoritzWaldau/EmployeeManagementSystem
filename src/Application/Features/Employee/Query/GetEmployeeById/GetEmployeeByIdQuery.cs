using Application.Models.Employee;

namespace Application.Features.Employee.Query.GetEmployeeById;

public sealed record GetEmployeeByIdQuery(Guid Id) : IQuery<GetEmployeeByIdQueryResult>;

public sealed record GetEmployeeByIdQueryResult(EmployeeResponse Response);