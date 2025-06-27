namespace Application.Features.Employee.Command.UpdateEmployee;

public class UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
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
        
        await unitOfWork.Employees.UpdateAsync(entity, cancellationToken);
        var mappedResponse = mapper.Map<EmployeeResponse>(entity);
        return Result<EmployeeResponse>.Success(mappedResponse);
    }
}