namespace Application.Features.Employee.Command.DeleteEmployee;

public sealed class DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteEmployeeCommand, Result<EmployeeResponse>>
{
    public async Task<Result<EmployeeResponse>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Employees.DeleteAsync(request.Id, cancellationToken);
        return result.IsSuccess 
            ? Result<EmployeeResponse>.Success() 
            : Result<EmployeeResponse>.Failure(result.ErrorMessage);
    }
}