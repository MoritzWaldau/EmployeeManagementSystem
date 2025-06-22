using Application.Models.Employee;
using Application.Models.Pagination;

namespace Application.Features.Employee.Query.GetAllEmployees;

public sealed class GetAllEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IQueryHandler<GetAllEmployeesQuery, GetAllEmployeesQueryResult>
{
    public async Task<GetAllEmployeesQueryResult> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var totalEmployees = await unitOfWork.Employees.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalEmployees / request.Request.PageSize);
        var employees = await unitOfWork.Employees.GetAllAsync(request.Request.PageIndex, request.Request.PageSize, cancellationToken);
        var mappedEmployees = mapper.Map<List<EmployeeResponse>>(employees);
        
        return new GetAllEmployeesQueryResult(
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