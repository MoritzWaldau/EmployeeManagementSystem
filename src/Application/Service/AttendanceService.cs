namespace Application.Service;

public sealed class AttendanceService(
    IAttendanceRepository attendanceRepository, 
    IMapper mapper,
    IValidator<AttendanceRequest> validator,
    HybridCache cache)
    : IAttendanceService
{
    public async Task<Result<PaginationResponse<AttendanceResponse>>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        var errorMessage = "";

        var cachedValue = await cache.GetOrCreateAsync(
            $"{CacheKeys.AttendanceKey}{paginationRequest.PageIndex}{paginationRequest.PageSize}",
            async ct =>
            {
                var totalAttendances = await attendanceRepository.CountAsync(ct);
                var totalPages = (int)Math.Ceiling((double)totalAttendances / paginationRequest.PageSize);
                var result = await attendanceRepository.GetAllAsync(paginationRequest.PageIndex, paginationRequest.PageSize, ct);
                
                if (!result.IsSuccess || !result.HasValue)
                {
                    errorMessage = result.ErrorMessage;
                    return null;
                }
                var mappedAttendances = mapper.Map<List<AttendanceResponse>>(result.Value);
                return new PaginationResponse<AttendanceResponse>(
                    paginationRequest.PageIndex,
                    paginationRequest.PageSize,
                    paginationRequest.PageIndex < totalPages,
                    paginationRequest.PageIndex > 1,
                    totalPages,
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

    public async Task<Result<AttendanceResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var errorMessage = "";
        var mappedAttendance = await cache.GetOrCreateAsync($"{CacheKeys.AttendanceKey}{id}", async ct =>
            {
                var resultFromDb = await attendanceRepository.GetByIdAsync(id, ct);
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

    public async Task<Result<AttendanceResponse>> CreateAsync(AttendanceRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<AttendanceResponse>.Failure(validationResult.Errors);
        }
        
        var attendance = mapper.Map<Domain.Entities.Attendance>(request);
        var result = await attendanceRepository.CreateAsync(attendance, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<AttendanceResponse>.Failure(result.ErrorMessage);
        }

        var response = mapper.Map<AttendanceResponse>(attendance);
        await cache.RemoveByTagAsync(CacheTags.AttendanceTag, cancellationToken);
        return Result<AttendanceResponse>.Success(response);
    }

    public async Task<Result<AttendanceResponse>> UpdateAsync(Guid id, AttendanceRequest request, CancellationToken cancellationToken = default)
    {
        var result = await attendanceRepository.GetByIdAsync(id, cancellationToken);
        
        if(!result.IsSuccess || !result.HasValue) 
        {
            return Result<AttendanceResponse>.Failure(result.ErrorMessage);
        }
        
        var entity = result.Value!;
        
        entity.Date = request.Date ?? entity.Date;
        entity.Status = request.Status ?? entity.Status;
        entity.CheckInTime = request.CheckInTime ?? entity.CheckInTime;
        entity.CheckOutTime = request.CheckOutTime ?? entity.CheckOutTime;
        
        var updatedResult = await attendanceRepository.UpdateAsync(entity, cancellationToken);
        if (!updatedResult.IsSuccess)
        {
            return Result<AttendanceResponse>.Failure(updatedResult.ErrorMessage);
        }
        var mappedResponse = mapper.Map<AttendanceResponse>(entity);
        await cache.RemoveByTagAsync(CacheTags.AttendanceTag, cancellationToken: cancellationToken);
        return Result<AttendanceResponse>.Success(mappedResponse);
    }

    public async Task<Result<AttendanceResponse>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await attendanceRepository.DeleteAsync(id, cancellationToken);

        if (!result.IsSuccess) return Result<AttendanceResponse>.Failure(result.ErrorMessage);

        await cache.RemoveByTagAsync(CacheTags.AttendanceTag, cancellationToken);
        return Result<AttendanceResponse>.Success();
    }
}