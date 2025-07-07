namespace Application.Features.Attendance.Query.GetAllAttendance;

public record GetAllAttendancesQuery(PaginationRequest Request) : IQuery<Result<PaginationResponse<AttendanceResponse>>>;
