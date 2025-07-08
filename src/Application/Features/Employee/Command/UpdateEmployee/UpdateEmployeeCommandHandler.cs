using Application.Common;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.Features.Employee.Command.UpdateEmployee;

public class UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, HybridCache cache)
    : ICommandHandler<UpdateEmployeeCommand, Result<EmployeeResponse>>
{
    public async Task<Result<EmployeeResponse>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Employees.GetByIdAsync(request.Id, cancellationToken);
        
        if(!result.IsSuccess) 
        {
            return Result<EmployeeResponse>.Failure(result.ErrorMessage);
        }

        var entity = result.Value!;
        
        entity.FirstName = request.Request.FirstName ?? entity.FirstName;
        entity.LastName = request.Request.LastName ?? entity.LastName;
        entity.Email = request.Request.Email ?? entity.Email;
        
        var updateResult = await unitOfWork.Employees.UpdateAsync(entity, cancellationToken);
        
        if (!updateResult.IsSuccess)
        {
            return Result<EmployeeResponse>.Failure(updateResult.ErrorMessage);
        }
        
        var mappedResponse = mapper.Map<EmployeeResponse>(entity);
        await cache.RemoveByTagAsync(CacheKeys.EmployeeKey, cancellationToken: cancellationToken);
        return Result<EmployeeResponse>.Success(mappedResponse);
    }
}