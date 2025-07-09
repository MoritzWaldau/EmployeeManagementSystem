namespace Application.Features.Attendance.Query.GetAttendanceById;

public sealed class GetAttendanceByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, HybridCache cache) 
    : IQueryHandler<GetAttendanceByIdQuery, Result<AttendanceResponse>>
{
    public async Task<Result<AttendanceResponse>> Handle(GetAttendanceByIdQuery request, CancellationToken cancellationToken)
    {
        var errorMessage = "";
        var mappedAttendance = await cache.GetOrCreateAsync($"{CacheKeys.AttendanceKey}{request.Id}", async ct =>
        {
            var resultFromDb = await unitOfWork.Attendances.GetByIdAsync(request.Id, ct);
            if (resultFromDb is { IsSuccess: true, HasValue: true })
                return mapper.Map<AttendanceResponse>(resultFromDb.Value);
            
            errorMessage = resultFromDb.ErrorMessage;
            return null;
        }, 
        tags: [CacheTags.AttendanceTag], 
        cancellationToken: cancellationToken);

        return mappedAttendance is null 
            ? Result<AttendanceResponse>.Failure(errorMessage) 
            : Result<AttendanceResponse>.Success(mappedAttendance);
    }
}