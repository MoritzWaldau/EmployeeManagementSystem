namespace Application.Features.Attendance.Command.DeleteAttendance;

public record DeleteAttendanceCommand(Guid Id) : ICommand<Result<AttendanceResponse>>;