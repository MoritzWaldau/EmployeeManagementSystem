using Application.Features.Employee.Command.DeleteEmployee;
using Application.Features.Employee.Command.UpdateEmployee;
using Application.Models.Pagination;
using Application.Models.Payroll;

namespace API.Endpoints;

public sealed class EmployeeEndpoint : ICarterModule
{
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/employee");
        
        group.MapGet("/", GetAllEmployees)
            .WithSummary("Get All Employees")
            .WithName(nameof(GetAllEmployees))
            .Produces<PaginationResponse<EmployeeResponse>>();
        
        group.MapGet("/{id}", GetEmployeeById)
            .WithSummary("Get Employee By Id")
            .WithName(nameof(GetEmployeeById))
            .Produces<EmployeeResponse>();
        
        group.MapPost("/", CreateEmployee)
            .WithSummary("Create Employee")
            .WithName(nameof(CreateEmployee))
            .Accepts<EmployeeRequest>("application/json")
            .Produces<EmployeeResponse>(StatusCodes.Status201Created);
        
        group.MapPut("/{id}", UpdateEmployee)
            .WithSummary("Update Employee")
            .WithName(nameof(UpdateEmployee))
            .Accepts<EmployeeRequest>("application/json")
            .Produces<EmployeeResponse>();
        
        group.MapDelete("/{id}", DeleteEmployee)
            .WithSummary("Delete Employee")
            .WithName(nameof(DeleteEmployee))
            .Produces(StatusCodes.Status204NoContent);
    }
    
    private static async Task<IResult> GetAllEmployees([AsParameters] PaginationRequest request, ISender sender)
    {
        var result = await sender.Send(new GetAllEmployeesQuery(request));
        return Results.Ok(result.Response);
    }

    private static async Task<IResult> GetEmployeeById(ISender sender, Guid id)
    {
        var result = await sender.Send(new GetEmployeeByIdQuery(id));
        return Results.Ok(result.Response);
    }
    private static async Task<IResult> CreateEmployee(EmployeeRequest request, ISender sender)
    {
        var result = await sender.Send(new CreateEmployeeCommand(request));
        return Results.Created($"/api/employee/{result.Response.Id}", result.Response);
    }
    
    private static async Task<IResult> UpdateEmployee(Guid id, EmployeeRequest request, ISender sender)
    {
        var result = await sender.Send(new UpdateEmployeeCommand(id, request));
        return Results.Ok(result.Response);
    }
    
    private static async Task<IResult> DeleteEmployee(Guid id, ISender sender)
    {
        await sender.Send(new DeleteEmployeeCommand(id));
        return Results.NoContent();
    }
}