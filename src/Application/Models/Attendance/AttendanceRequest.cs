namespace Application.Models.Attendance;

public sealed record AttendanceRequest
{
    [JsonPropertyName("employeeId")]
    public required Guid EmployeeId { get; init; }
    
    [JsonPropertyName("date")]
    public DateOnly? Date { get; init; }
    
    [JsonPropertyName("checkInTime")]
    public TimeSpan? CheckInTime { get; init; }
    
    [JsonPropertyName("checkOutTime")]
    public TimeSpan? CheckOutTime { get; init; }
    
    [JsonPropertyName("status")]
    public Status? Status { get; init; }
}