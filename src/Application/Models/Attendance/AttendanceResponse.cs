namespace Application.Models.Attendance;

public sealed record AttendanceResponse : BaseResponse
{
    public required DateOnly Date { get; init; }
    public required TimeSpan CheckInTime { get; init; }
    public required TimeSpan CheckOutTime { get; init; }
    public required TimeSpan WorkDuration { get; init; }
    public required Status Status { get; init; }
}