namespace Application.Features.Attendance.Command.DeleteAttendance;

public sealed class DeleteAttendanceCommandHandler(IUnitOfWork unitOfWork, HybridCache cache) : ICommandHandler<DeleteAttendanceCommand, Result<AttendanceResponse>>
{
    public async Task<Result<AttendanceResponse>> Handle(DeleteAttendanceCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Attendances.DeleteAsync(request.Id, cancellationToken);

        if (!result.IsSuccess) return Result<AttendanceResponse>.Failure(result.ErrorMessage);

        await cache.RemoveByTagAsync(CacheTags.AttendanceTag, cancellationToken);
        return Result<AttendanceResponse>.Success();
    }
}