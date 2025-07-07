namespace Application.Features.Attendance.Command.DeleteAttendance;

public sealed class DeleteAttendanceCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteAttendanceCommand, Result<AttendanceResponse>>
{
    public async Task<Result<AttendanceResponse>> Handle(DeleteAttendanceCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Attendances.DeleteAsync(request.Id, cancellationToken);
        return result.IsSuccess 
            ? Result<AttendanceResponse>.Success() 
            : Result<AttendanceResponse>.Failure(result.ErrorMessage);
    }
}