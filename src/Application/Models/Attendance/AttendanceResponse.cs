namespace Application.Models.Attendance;

public sealed record AttendanceResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime? ModifiedAt,
    bool IsActive,
    DateOnly Date,
    TimeSpan CheckInTime,
    TimeSpan CheckOutTime,
    TimeSpan WorkDuration,
    Status Status
) : BaseResponse(Id, CreatedAt, ModifiedAt, IsActive);