namespace Application.Models.Employee;

public sealed record EmployeeResponse : BaseResponse
{
    [JsonPropertyName("firstName")]
    public required string FirstName { get; init; }
    
    [JsonPropertyName("lastName")]
    public required string LastName { get; init; }
    
    [JsonPropertyName("email")]
    public required string Email { get; init; }
    
    [JsonPropertyName("payrolls")]
    public IEnumerable<PayrollResponse>? Payrolls { get; init; }

    [JsonPropertyName("attendance")]
    public IEnumerable<AttendanceResponse>? Attendances { get; set; }

}