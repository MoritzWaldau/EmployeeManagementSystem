namespace Application.Features.Employee.Command.CreateEmployee;

public sealed class CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : ICommandHandler<CreateEmployeeCommand, CreateEmployeeCommandResult>
{
    public async Task<CreateEmployeeCommandResult> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = mapper.Map<Domain.Entities.Employee>(request.Request);
        await unitOfWork.Employees.CreateAsync(employee, cancellationToken);
        var mappedEmployee = mapper.Map<EmployeeResponse>(employee);
        return new CreateEmployeeCommandResult(mappedEmployee);
    }
}