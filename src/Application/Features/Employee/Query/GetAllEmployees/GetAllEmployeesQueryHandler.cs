namespace Application.Features.Employee.Query.GetAllEmployees;

public sealed class GetAllEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, HybridCache cache) 
    : IQueryHandler<GetAllEmployeesQuery, Result<PaginationResponse<EmployeeResponse>>>
{
    public async Task<Result<PaginationResponse<EmployeeResponse>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var errorMessage = "";
        var cachedValue = await cache.GetOrCreateAsync(
            $"{CacheKeys.EmployeeKey}{request.Request.PageIndex}{request.Request.PageSize}",
            async ct =>
            {
                var totalEmployees = await unitOfWork.Employees.CountAsync(ct);
                var totalPages = (int)Math.Ceiling((double)totalEmployees / request.Request.PageSize);
                var result = await unitOfWork.Employees.GetAllAsync(request.Request.PageIndex, request.Request.PageSize, ct);
                
                if (!result.IsSuccess || !result.HasValue)
                {
                    errorMessage = result.ErrorMessage;
                    return null;
                }
                var mappedEmployees = mapper.Map<List<EmployeeResponse>>(result.Value);

                return new PaginationResponse<EmployeeResponse>(
                    request.Request.PageIndex,
                    request.Request.PageSize,
                    request.Request.PageIndex < totalPages,
                    request.Request.PageIndex > 1,
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
}