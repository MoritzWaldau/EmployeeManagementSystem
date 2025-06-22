namespace Application.Features.Employee.Query.GetEmployeeById;

public class GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IQueryHandler<GetEmployeeByIdQuery, GetEmployeeByIdQueryResult>
{
    public async Task<GetEmployeeByIdQueryResult> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await unitOfWork.Employees.GetByIdAsync(request.Id, cancellationToken);
        var mappedEmployee = mapper.Map<EmployeeResponse>(employee);
        return new GetEmployeeByIdQueryResult(mappedEmployee);
    }
}