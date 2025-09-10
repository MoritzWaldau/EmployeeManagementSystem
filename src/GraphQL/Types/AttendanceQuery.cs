namespace GraphQL.Types;

[QueryType]
public static class AttendanceQuery
{
    public static async Task<AttendanceResponse> GetAttendanceByIdAsync(
        Guid id,
        [Service] IAttendanceService attendanceService,
        CancellationToken cancellationToken = default)
    {
        var result = await attendanceService.GetByIdAsync(id, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }

    public static async Task<PaginationResponse<AttendanceResponse>> GetAllAttendancesAsync(
        PaginationRequest request,
        [Service] IAttendanceService attendanceService,
        CancellationToken cancellationToken = default)
    {
        var result = await attendanceService.GetAllAsync(request, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }
}
