namespace Application.Features.Attendance.Query.GetAttendanceById;

public sealed class GetAttendanceByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IQueryHandler<GetAttendanceByIdQuery, Result<AttendanceResponse>>
{
    public async Task<Result<AttendanceResponse>> Handle(GetAttendanceByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Attendances.GetByIdAsync(request.Id, cancellationToken);
        if (!result.IsSuccess || result.HasValue)
        {
            return Result<AttendanceResponse>.Failure(result.ErrorMessage);
        }
        
        var mappedAttendance = mapper.Map<AttendanceResponse>(result.Value);
        return Result<AttendanceResponse>.Success(mappedAttendance);
    }
}