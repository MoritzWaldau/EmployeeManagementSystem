namespace Application.Models.Employee;

public sealed record EmployeeRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public bool? IsActive { get; set; }
}