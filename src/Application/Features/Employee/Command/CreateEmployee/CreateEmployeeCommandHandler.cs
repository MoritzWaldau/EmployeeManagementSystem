namespace Application.Features.Employee.Command.CreateEmployee;

public sealed class CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : ICommandHandler<CreateEmployeeCommand, Result<EmployeeResponse>>
{
    public async Task<Result<EmployeeResponse>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = mapper.Map<Domain.Entities.Employee>(request.Employee);
        var result = await unitOfWork.Employees.CreateAsync(employee, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<EmployeeResponse>.Failure(result.ErrorMessage);
        }
        
        var mappedEmployee = mapper.Map<EmployeeResponse>(employee);
        return Result<EmployeeResponse>.Success(mappedEmployee);
    }
}