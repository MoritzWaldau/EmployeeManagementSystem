using Refit;
using Shared.Models.Employee;
using Shared.Models.Pagination;

namespace BlazorApp.Backend;

public interface IApi
{
    [Get("/api/employee")]
    Task<PaginationResponse<EmployeeResponse>> GetEmployees(int pageIndex = 1, int pageSize = 10);
}