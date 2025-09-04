using Refit;
using Shared.Models.Attendance;
using Shared.Models.Employee;
using Shared.Models.Pagination;

namespace BlazorApp.Backend;

public interface IApi
{
    [Get("/api/employee")]
    Task<PaginationResponse<EmployeeResponse>> GetEmployees(int pageIndex = 1, int pageSize = 10);
    
    [Get("/api/employee/{id}")]
    Task<EmployeeResponse> GetEmployeeById(Guid id);
    
    [Post("/api/employee")]
    Task<EmployeeResponse> CreateEmployee([Body] EmployeeRequest employeeRequest);
    
    [Put("/api/employee/{id}")]
    Task<EmployeeResponse> UpdateEmployee(Guid id, [Body] EmployeeRequest employeeRequest);
    
    [Delete("/api/employee/{id}")]
    Task DeleteEmployee(Guid id);
    
    [Get("/api/attendance")]
    Task<PaginationResponse<AttendanceResponse>> GetAttendances(int pageIndex = 1, int pageSize = 10);
    
    [Get("/api/attendance/{id}")]
    Task<AttendanceResponse> GetAttendanceById(Guid id);
    
    [Post("/api/attendance")]
    Task<AttendanceResponse> CreateAttendance([Body] AttendanceRequest attendanceRequest);
    
    [Put("/api/attendance/{id}")]
    Task<AttendanceResponse> UpdateAttendance(Guid id, [Body] AttendanceRequest attendanceRequest);
    
    [Delete("/api/attendance/{id}")]
    Task DeleteAttendance(Guid id);
    
    [Get("/api/data/{count}")]
    Task<string> GenerateFakeData(int count);
}