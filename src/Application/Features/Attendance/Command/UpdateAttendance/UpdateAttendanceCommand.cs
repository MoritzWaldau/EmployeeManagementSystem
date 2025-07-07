namespace Application.Features.Attendance.Command.UpdateAttendance;

public sealed record UpdateAttendanceCommand(Guid Id, AttendanceRequest Attendance) : ICommand<Result<AttendanceResponse>>;