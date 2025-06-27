namespace Application.Features.Employee.Query.GetAllEmployees;

public sealed class GetAllEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IQueryHandler<GetAllEmployeesQuery, Result<PaginationResponse<EmployeeResponse>>>
{
    public async Task<Result<PaginationResponse<EmployeeResponse>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var totalEmployees = await unitOfWork.Employees.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalEmployees / request.Request.PageSize);
        var result = await unitOfWork.Employees.GetAllAsync(request.Request.PageIndex, request.Request.PageSize, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return Result<PaginationResponse<EmployeeResponse>>.Failure(result.ErrorMessage);
        }
        
        var employees = result.Value;
        var mappedEmployees = mapper.Map<List<EmployeeResponse>>(employees);
        return Result<PaginationResponse<EmployeeResponse>>.Success(
            new PaginationResponse<EmployeeResponse>(
                request.Request.PageIndex,
                request.Request.PageSize,
                request.Request.PageIndex < totalPages,
                request.Request.PageIndex > 1,
                mappedEmployees
            )
        );
    }
}