namespace Application.Features.Employee.Command.DeleteEmployee;

public sealed class DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork, HybridCache cache) : ICommandHandler<DeleteEmployeeCommand, Result<EmployeeResponse>>
{
    public async Task<Result<EmployeeResponse>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Employees.DeleteAsync(request.Id, cancellationToken);

        if (!result.IsSuccess) return Result<EmployeeResponse>.Failure(result.ErrorMessage);
        
        await cache.RemoveByTagAsync(CacheTags.EmployeeTag, cancellationToken);
        return Result<EmployeeResponse>.Success();
    }
}