namespace Application.Models.Employee;

public sealed record EmployeeRequest
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }
    
    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }
    
    [JsonPropertyName("email")]
    public string? Email { get; init; }
    
    [JsonPropertyName("isActive")]
    public bool? IsActive { get; set; }
}