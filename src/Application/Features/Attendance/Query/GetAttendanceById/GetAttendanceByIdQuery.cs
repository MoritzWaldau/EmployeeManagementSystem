namespace Application.Features.Attendance.Query.GetAttendanceById;

public sealed record GetAttendanceByIdQuery(Guid Id) : IQuery<Result<AttendanceResponse>>;