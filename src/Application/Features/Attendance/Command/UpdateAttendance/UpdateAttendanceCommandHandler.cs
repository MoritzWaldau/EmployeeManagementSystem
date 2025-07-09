namespace Application.Features.Attendance.Command.UpdateAttendance;

public sealed class UpdateAttendanceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, HybridCache cache)
    : ICommandHandler<UpdateAttendanceCommand, Result<AttendanceResponse>>
{
    public async Task<Result<AttendanceResponse>> Handle(UpdateAttendanceCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Attendances.GetByIdAsync(request.Id, cancellationToken);
        
        if(!result.IsSuccess || !result.HasValue) 
        {
            return Result<AttendanceResponse>.Failure(result.ErrorMessage);
        }
        
        var entity = result.Value!;
        
        entity.Date = request.Attendance.Date ?? entity.Date;
        entity.Status = request.Attendance.Status ?? entity.Status;
        entity.CheckInTime = request.Attendance.CheckInTime ?? entity.CheckInTime;
        entity.CheckOutTime = request.Attendance.CheckOutTime ?? entity.CheckOutTime;
        
        var updatedResult = await unitOfWork.Attendances.UpdateAsync(entity, cancellationToken);
        if (!updatedResult.IsSuccess)
        {
            return Result<AttendanceResponse>.Failure(updatedResult.ErrorMessage);
        }
        var mappedResponse = mapper.Map<AttendanceResponse>(entity);
        await cache.RemoveByTagAsync(CacheTags.AttendanceTag, cancellationToken: cancellationToken);
        return Result<AttendanceResponse>.Success(mappedResponse);
    }
}