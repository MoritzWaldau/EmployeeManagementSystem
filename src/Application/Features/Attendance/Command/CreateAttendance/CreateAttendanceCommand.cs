using Application.Models.Attendance;

namespace Application.Features.Attendance.Command.CreateAttendance;

public sealed record CreateAttendanceCommand(AttendanceRequest Attendance) : ICommand<Result<AttendanceResponse>>;