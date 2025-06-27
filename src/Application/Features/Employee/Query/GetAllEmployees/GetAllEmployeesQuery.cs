namespace Application.Features.Employee.Query.GetAllEmployees;

public sealed record GetAllEmployeesQuery(PaginationRequest Request) : IQuery<Result<PaginationResponse<EmployeeResponse>>>;
