namespace Application.Features.Employee.Query.GetEmployeeById;

public class GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IQueryHandler<GetEmployeeByIdQuery, Result<EmployeeResponse>>
{
    public async Task<Result<EmployeeResponse>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Employees.GetByIdAsync(request.Id, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<EmployeeResponse>.Failure(result.ErrorMessage);
        }

        var employee = result.Value!;
        var mappedEmployee = mapper.Map<EmployeeResponse>(employee);
        return Result<EmployeeResponse>.Success(mappedEmployee);
    }
}