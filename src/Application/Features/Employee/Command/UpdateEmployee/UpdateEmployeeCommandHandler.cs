namespace Application.Features.Employee.Command.UpdateEmployee;

public class UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : ICommandHandler<UpdateEmployeeCommand, UpdateEmployeeCommandResponse>
{
    public async Task<UpdateEmployeeCommandResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.Employees.GetByIdAsync(request.Id, cancellationToken);

        entity.FirstName = request.Request.FirstName ?? entity.FirstName;
        entity.LastName = request.Request.LastName ?? entity.LastName;
        entity.Email = request.Request.Email ?? entity.Email;
        
        await unitOfWork.Employees.UpdateAsync(entity, cancellationToken);
        var mappedResponse = mapper.Map<EmployeeResponse>(entity);
        return new UpdateEmployeeCommandResponse(mappedResponse);
    }
}