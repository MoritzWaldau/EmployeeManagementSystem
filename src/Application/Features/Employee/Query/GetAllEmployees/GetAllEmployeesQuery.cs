using Application.Models.Employee;
using Application.Models.Pagination;

namespace Application.Features.Employee.Query.GetAllEmployees;

public sealed record GetAllEmployeesQuery(PaginationRequest Request) : IQuery<GetAllEmployeesQueryResult>;

public sealed record GetAllEmployeesQueryResult(PaginationResponse<EmployeeResponse> Response);