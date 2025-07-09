namespace Application.Features.Attendance.Query.GetAllAttendance;

public sealed class GetAllAttendancesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, HybridCache cache)
    : IQueryHandler<GetAllAttendancesQuery, Result<PaginationResponse<AttendanceResponse>>>
{
    public async Task<Result<PaginationResponse<AttendanceResponse>>> Handle(GetAllAttendancesQuery request, CancellationToken cancellationToken)
    {
        var errorMessage = "";

        var cachedValue = await cache.GetOrCreateAsync(
            $"{CacheKeys.AttendanceKey}{request.Request.PageIndex}{request.Request.PageSize}",
            async ct =>
            {
                var totalAttendances = await unitOfWork.Attendances.CountAsync(ct);
                var totalPages = (int)Math.Ceiling((double)totalAttendances / request.Request.PageSize);
                var result = await unitOfWork.Attendances.GetAllAsync(request.Request.PageIndex, request.Request.PageSize, ct);
                
                if (!result.IsSuccess || !result.HasValue)
                {
                    errorMessage = result.ErrorMessage;
                    return null;
                }
                var mappedAttendances = mapper.Map<List<AttendanceResponse>>(result.Value);
                return new PaginationResponse<AttendanceResponse>(
                    request.Request.PageIndex,
                    request.Request.PageSize,
                    request.Request.PageIndex < totalPages,
                    request.Request.PageIndex > 1,
                    mappedAttendances
                );
            },
            tags: [CacheTags.AttendanceTag],
            cancellationToken: cancellationToken
        );

        return cachedValue is null 
            ? Result<PaginationResponse<AttendanceResponse>>.Failure(errorMessage)
            : Result<PaginationResponse<AttendanceResponse>>.Success(cachedValue);
    }
}