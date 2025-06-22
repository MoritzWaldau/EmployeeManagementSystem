namespace Application.Features.Employee.Command.DeleteEmployee;

public sealed class DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteEmployeeCommand>
{
    public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.Employees.DeleteAsync(request.Id, cancellationToken);
        
        return Unit.Value;
    }
}