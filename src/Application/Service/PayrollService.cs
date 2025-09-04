namespace Application.Service;

//TODO: Create validator for PayrollRequest

public sealed class PayrollService(
    IPayrollRepository payrollRepository, 
    IMapper mapper,
    HybridCache cache) 
    : IPayrollService
{
    public async Task<Result<PaginationResponse<PayrollResponse>>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        var errorMessage = "";
        var cachedValue = await cache.GetOrCreateAsync(
            $"{CacheKeys.PayrollKey}{paginationRequest.PageIndex}{paginationRequest.PageSize}",
            async ct =>
            {
                var totalPayrolls = await payrollRepository.CountAsync(ct);
                var totalPages = (int)Math.Ceiling((double)totalPayrolls / paginationRequest.PageSize);
                var result = await payrollRepository.GetAllAsync(paginationRequest.PageIndex, paginationRequest.PageSize, ct);
                
                if (!result.IsSuccess || !result.HasValue)
                {
                    errorMessage = result.ErrorMessage;
                    return null;
                }
                
                var mappedPayrolls = mapper.Map<List<PayrollResponse>>(result.Value);
                
                return new PaginationResponse<PayrollResponse>(
                    paginationRequest.PageIndex,
                    paginationRequest.PageSize,
                    paginationRequest.PageIndex < totalPages,
                    paginationRequest.PageIndex > 1,
                    totalPages,
                    mappedPayrolls
                );
            },
            tags: [CacheTags.PayrollTag],
            cancellationToken: cancellationToken
        );

        return cachedValue is null
            ? Result<PaginationResponse<PayrollResponse>>.Failure(errorMessage)
            : Result<PaginationResponse<PayrollResponse>>.Success(cachedValue);
    }

    public async Task<Result<PayrollResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var errorMessage = "";
        var mappedEntity = await cache.GetOrCreateAsync($"{CacheKeys.PayrollKey}{id}", async ct =>
        {
            var resultFromDb = await payrollRepository.GetByIdAsync(id, ct);
            if (resultFromDb is { IsSuccess: true, HasValue: true })
                return mapper.Map<PayrollResponse>(resultFromDb.Value);

            errorMessage = resultFromDb.ErrorMessage;
            return null;
        }, tags: [CacheTags.PayrollTag], cancellationToken: cancellationToken);

        return mappedEntity is null 
            ? Result<PayrollResponse>.Failure(errorMessage) 
            : Result<PayrollResponse>.Success(mappedEntity);
    }

    public async Task<Result<PayrollResponse>> CreateAsync(PayrollRequest request, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Domain.Entities.Payroll>(request);
        var result = await payrollRepository.CreateAsync(entity, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<PayrollResponse>.Failure(result.ErrorMessage);
        }
        var mappedPayroll = mapper.Map<PayrollResponse>(entity);
        await cache.RemoveByTagAsync(CacheTags.PayrollTag, cancellationToken);
        return Result<PayrollResponse>.Success(mappedPayroll);
    }

    public async Task<Result<PayrollResponse>> UpdateAsync(Guid id, PayrollRequest request, CancellationToken cancellationToken = default)
    {
        var result = await payrollRepository.GetByIdAsync(id, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return Result<PayrollResponse>.Failure(result.ErrorMessage);
        }
        
        var entity = result.Value!;
        
        entity.EmployeeId = request.EmployeeId ?? entity.EmployeeId;
        entity.Year = request.Year ?? entity.Year;
        entity.Month = request.Month ?? entity.Month;
        entity.GrossSalary = request.GrossSalary ?? entity.GrossSalary;
        entity.NetSalary = request.NetSalary ?? entity.NetSalary;
        
        var updateResult = await payrollRepository.UpdateAsync(entity, cancellationToken);

        if (!updateResult.IsSuccess)
        {
            return Result<PayrollResponse>.Failure(updateResult.ErrorMessage);
        }

        var mappedResponse = mapper.Map<PayrollResponse>(entity);
        await cache.RemoveByTagAsync(CacheTags.PayrollTag, cancellationToken: cancellationToken);
        return Result<PayrollResponse>.Success(mappedResponse);
    }

    public async Task<Result<PayrollResponse>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await payrollRepository.DeleteAsync(id, cancellationToken);

        if (!result.IsSuccess) return Result<PayrollResponse>.Failure(result.ErrorMessage);

        await cache.RemoveByTagAsync(CacheTags.PayrollTag, cancellationToken);
        return Result<PayrollResponse>.Success();
    }
}