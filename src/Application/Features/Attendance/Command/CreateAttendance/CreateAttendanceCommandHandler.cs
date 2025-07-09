namespace Application.Features.Attendance.Command.CreateAttendance;

public sealed class CreateAttendanceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, HybridCache cache) 
    : ICommandHandler<CreateAttendanceCommand, Result<AttendanceResponse>>
{
    public async Task<Result<AttendanceResponse>> Handle(CreateAttendanceCommand request, CancellationToken cancellationToken)
    {
        var attendance = mapper.Map<Domain.Entities.Attendance>(request.Attendance);
        var result = await unitOfWork.Attendances.CreateAsync(attendance, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<AttendanceResponse>.Failure(result.ErrorMessage);
        }

        var response = mapper.Map<AttendanceResponse>(attendance);
        await cache.RemoveByTagAsync(CacheTags.AttendanceTag, cancellationToken);
        return Result<AttendanceResponse>.Success(response);
    }
}