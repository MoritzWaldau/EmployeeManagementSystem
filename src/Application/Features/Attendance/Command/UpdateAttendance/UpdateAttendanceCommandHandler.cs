namespace Application.Features.Attendance.Command.UpdateAttendance;

public sealed class UpdateAttendanceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
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
        
        await unitOfWork.Attendances.UpdateAsync(entity, cancellationToken);
        var mappedResponse = mapper.Map<AttendanceResponse>(entity);
        return Result<AttendanceResponse>.Success(mappedResponse);
    }
}