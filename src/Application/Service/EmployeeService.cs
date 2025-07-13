using Application.Abstraction.Service;

namespace Application.Service;

public sealed class EmployeeService(
    IUnitOfWork unitOfWork, 
    IMapper mapper,
    IValidator<EmployeeRequest> validator,
    HybridCache cache) 
    : IEmployeeService
{
    public async Task<Result<PaginationResponse<EmployeeResponse>>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        var errorMessage = "";
        var cachedValue = await cache.GetOrCreateAsync(
            $"{CacheKeys.EmployeeKey}{paginationRequest.PageIndex}{paginationRequest.PageSize}",
            async ct =>
            {
                var totalEmployees = await unitOfWork.Employees.CountAsync(ct);
                var totalPages = (int)Math.Ceiling((double)totalEmployees / paginationRequest.PageSize);
                var result = await unitOfWork.Employees.GetAllAsync(paginationRequest.PageIndex, paginationRequest.PageSize, ct);
                
                if (!result.IsSuccess || !result.HasValue)
                {
                    errorMessage = result.ErrorMessage;
                    return null;
                }
                var mappedEmployees = mapper.Map<List<EmployeeResponse>>(result.Value);

                return new PaginationResponse<EmployeeResponse>(
                    paginationRequest.PageIndex,
                    paginationRequest.PageSize,
                    paginationRequest.PageIndex < totalPages,
                    paginationRequest.PageIndex > 1,
                    mappedEmployees
                );
            },
            tags: [CacheTags.EmployeeTag],
            cancellationToken: cancellationToken
        );
        
        return cachedValue is null 
            ? Result<PaginationResponse<EmployeeResponse>>.Failure(errorMessage)
            : Result<PaginationResponse<EmployeeResponse>>.Success(cachedValue);
    }

    public async Task<Result<EmployeeResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var errorMessage = "";
        var mappedEntity = await cache.GetOrCreateAsync($"{CacheKeys.EmployeeKey}{id}", async ct =>
        {
            var resultFromDb = await unitOfWork.Employees.GetByIdAsync(id, ct);
            if (resultFromDb is { IsSuccess: true, HasValue: true })
                return mapper.Map<EmployeeResponse>(resultFromDb.Value);
        
            errorMessage = resultFromDb.ErrorMessage;
            return null;
        },
        tags: [CacheTags.EmployeeTag], 
        cancellationToken: cancellationToken);
        
        return mappedEntity is null 
            ? Result<EmployeeResponse>.Failure(errorMessage) 
            : Result<EmployeeResponse>.Success(mappedEntity);
    }

    public async Task<Result<EmployeeResponse>> CreateAsync(EmployeeRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<EmployeeResponse>.Failure(validationResult.Errors);
        }
        
        var employee = mapper.Map<Domain.Entities.Employee>(request);
        var result = await unitOfWork.Employees.CreateAsync(employee, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<EmployeeResponse>.Failure(result.ErrorMessage);
        }
        
        var mappedEmployee = mapper.Map<EmployeeResponse>(employee);
        await cache.RemoveByTagAsync(CacheTags.EmployeeTag, cancellationToken);
        return Result<EmployeeResponse>.Success(mappedEmployee);
    }

    public async Task<Result<EmployeeResponse>> UpdateAsync(Guid id, EmployeeRequest request, CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.Employees.GetByIdAsync(id, cancellationToken);
        
        if(!result.IsSuccess) 
        {
            return Result<EmployeeResponse>.Failure(result.ErrorMessage);
        }

        var entity = result.Value!;
        
        entity.FirstName = request.FirstName ?? entity.FirstName;
        entity.LastName = request.LastName ?? entity.LastName;
        entity.Email = request.Email ?? entity.Email;
        entity.IsActive = request.IsActive ?? entity.IsActive;
        
        var updateResult = await unitOfWork.Employees.UpdateAsync(entity, cancellationToken);
        
        if (!updateResult.IsSuccess)
        {
            return Result<EmployeeResponse>.Failure(updateResult.ErrorMessage);
        }
        
        var mappedResponse = mapper.Map<EmployeeResponse>(entity);
        await cache.RemoveByTagAsync(CacheTags.EmployeeTag, cancellationToken: cancellationToken);
        return Result<EmployeeResponse>.Success(mappedResponse);
    }

    public async Task<Result<EmployeeResponse>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.Employees.DeleteAsync(id, cancellationToken);
        
        if (!result.IsSuccess) return Result<EmployeeResponse>.Failure(result.ErrorMessage);
        
        await cache.RemoveByTagAsync(CacheTags.EmployeeTag, cancellationToken);
        return Result<EmployeeResponse>.Success();
    }
}