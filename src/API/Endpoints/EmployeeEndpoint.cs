namespace API.Endpoints;

public sealed class EmployeeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/employee");
        
        group.MapGet("/", GetAllEmployees)
            .WithSummary("Get all employees")
            .WithName(nameof(GetAllEmployees))
            .Produces<PaginationResponse<EmployeeResponse>>()
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapGet("/{id}", GetEmployeeById)
            .WithSummary("Get employee by id")
            .WithName(nameof(GetEmployeeById))
            .Produces<EmployeeResponse>()
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPost("/", CreateEmployee)
            .WithSummary("Create Employee")
            .WithName(nameof(CreateEmployee))
            .Accepts<EmployeeRequest>("application/json")
            .Produces<EmployeeResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPut("/{id}", UpdateEmployee)
            .WithSummary("Update employee")
            .WithName(nameof(UpdateEmployee))
            .Accepts<EmployeeRequest>("application/json")
            .Produces<EmployeeResponse>()
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapDelete("/{id}", DeleteEmployee)
            .WithSummary("Delete employee")
            .WithName(nameof(DeleteEmployee))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest);
    }
    
    private static async Task<IResult> GetAllEmployees([AsParameters] PaginationRequest request, [FromServices] IEmployeeService employeeService)
    {
        var result = await employeeService.GetAllAsync(request);
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails("api/employee", "Failed to get all employees")));
    }

    private static async Task<IResult> GetEmployeeById(Guid id, [FromServices] IEmployeeService employeeService)
    {
        var result = await employeeService.GetByIdAsync(id);
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails($"api/employee/{id}", "Failed to get employee by id")));
    }
    private static async Task<IResult> CreateEmployee(EmployeeRequest request, [FromServices] IEmployeeService employeeService)
    {
        var result = await employeeService.CreateAsync(request);
        return result.Match(x => Results.Created(
            $"/api/employee/{result.Value!.Id}", result.Value!), 
            err => Results.BadRequest(err.ToProblemDetails($"/api/employee", "Failed to create employee"))
        );
    }
    
    private static async Task<IResult> UpdateEmployee(Guid id, EmployeeRequest request, [FromServices] IEmployeeService employeeService)
    {
        var result = await employeeService.UpdateAsync(id, request);
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails($"api/employee/{id}", "Failed to update employee")));
    }
    
    private static async Task<IResult> DeleteEmployee(Guid id, [FromServices] IEmployeeService employeeService)
    {
        var result = await employeeService.DeleteAsync(id);
        return result.Match(_ => Results.NoContent(), err => Results.BadRequest(err.ToProblemDetails($"api/employee/{id}", "Failed to delete employee")));
    }
}