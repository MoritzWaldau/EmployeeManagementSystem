namespace Application.Models.Attendance;

public sealed record AttendanceRequest
{
    [JsonPropertyName("employeeId")]
    public required Guid EmployeeId { get; init; }
    
    [JsonPropertyName("date")]
    public required DateOnly Date { get; init; }
    
    [JsonPropertyName("checkInTime")]
    public required TimeSpan CheckInTime { get; init; }
    
    [JsonPropertyName("checkOutTime")]
    public required TimeSpan CheckOutTime { get; init; }
    
    [JsonPropertyName("status")]
    public required Status Status { get; init; }
}