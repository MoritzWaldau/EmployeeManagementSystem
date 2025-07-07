namespace Application.Features.Attendance.Query.GetAllAttendance;

public sealed class GetAllAttendancesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IQueryHandler<GetAllAttendancesQuery, Result<PaginationResponse<AttendanceResponse>>>
{
    public async Task<Result<PaginationResponse<AttendanceResponse>>> Handle(GetAllAttendancesQuery request, CancellationToken cancellationToken)
    {
        var totalAttendances = await unitOfWork.Attendances.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalAttendances / request.Request.PageSize);
        var result = await unitOfWork.Attendances.GetAllAsync(request.Request.PageIndex, request.Request.PageSize, cancellationToken);
        
        if (!result.IsSuccess || !result.HasValue)
        {
            return Result<PaginationResponse<AttendanceResponse>>.Failure(result.ErrorMessage);
        }

        var mappedAttendances = mapper.Map<List<AttendanceResponse>>(result.Value);
        return Result<PaginationResponse<AttendanceResponse>>.Success(
            new PaginationResponse<AttendanceResponse>(
                request.Request.PageIndex,
                request.Request.PageSize,
                request.Request.PageIndex < totalPages,
                request.Request.PageIndex > 1,
                mappedAttendances
            )
        );
    }
}