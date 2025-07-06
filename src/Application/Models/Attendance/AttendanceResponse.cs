namespace Application.Models.Attendance;

public sealed record AttendanceResponse : BaseResponse
{
    public required DateOnly Date { get; init; }
    public required TimeOnly CheckInTime { get; init; }
    public required TimeOnly CheckOutTime { get; init; }
    public required double WorkDuration { get; init; }
    public required Status Status { get; init; }
}