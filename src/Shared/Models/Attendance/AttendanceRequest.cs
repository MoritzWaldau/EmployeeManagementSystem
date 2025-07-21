namespace Shared.Models.Attendance;

public sealed record AttendanceRequest(
    Guid EmployeeId,
    DateOnly? Date,
    TimeSpan? CheckInTime,
    TimeSpan? CheckOutTime,
    Status? Status
);