namespace Application.Models.Employee;

public sealed record EmployeeRequest(
    string? FirstName, 
    string? LastName, 
    string? Email, 
    bool? IsActive
);