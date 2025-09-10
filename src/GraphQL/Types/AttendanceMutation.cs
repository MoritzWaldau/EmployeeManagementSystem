namespace GraphQL.Types;

[MutationType]
public static class AttendanceMutation
{
    public static async Task<AttendanceResponse> CreateAttendanceAsync(
        AttendanceRequest request,
        [Service] IAttendanceService attendanceService,
        CancellationToken cancellationToken = default)
    {
        var result = await attendanceService.CreateAsync(request, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }

    public static async Task<AttendanceResponse> UpdateAttendanceAsync(
        Guid id,
        AttendanceRequest request,
        [Service] IAttendanceService attendanceService,
        CancellationToken cancellationToken = default)
    {
        var result = await attendanceService.UpdateAsync(id, request, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }

    public static async Task<AttendanceResponse> DeleteAttendanceAsync(
        Guid id,
        [Service] IAttendanceService attendanceService,
        CancellationToken cancellationToken = default)
    {
        var result = await attendanceService.DeleteAsync(id, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }
}
