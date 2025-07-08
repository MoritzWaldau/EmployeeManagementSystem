using Application.Common;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.Features.Employee.Query.GetEmployeeById;

public class GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, HybridCache cache) 
    : IQueryHandler<GetEmployeeByIdQuery, Result<EmployeeResponse>>
{
    public async Task<Result<EmployeeResponse>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var errorMessage = "";
        var mappedEntity = await cache.GetOrCreateAsync($"{CacheKeys.EmployeeKey}{request.Id}", async ct =>
        {
            var resultFromDb = await unitOfWork.Employees.GetByIdAsync(request.Id, ct);
            if (resultFromDb is { IsSuccess: true, HasValue: true })
                return mapper.Map<EmployeeResponse>(resultFromDb.Value);
            
            errorMessage = resultFromDb.ErrorMessage;
            return null;

        }, tags: [CacheTags.EmployeeTag], cancellationToken: cancellationToken);
        
       
        return mappedEntity is null 
            ? Result<EmployeeResponse>.Failure(errorMessage) 
            : Result<EmployeeResponse>.Success(mappedEntity);
    }
}