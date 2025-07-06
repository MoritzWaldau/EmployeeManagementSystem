using API.Extensions;
using Application.Features.Attendance.Command.CreateAttendance;
using Application.Models.Attendance;

namespace API.Endpoints;

public sealed class AttendanceEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/attendance");
        
        group.MapPost("/", CreateAttendance)
            .WithSummary("Create attendance")
            .WithName(nameof(CreateAttendance))
            .Accepts<EmployeeRequest>("application/json")
            .Produces<EmployeeResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
        
    }
    
    private static async Task<IResult> CreateAttendance(ISender sender, AttendanceRequest request)
    {
        var result = await sender.Send(new CreateAttendanceCommand(request));
        return result.Match(x => Results.Created(
                $"/api/attendance/{result.Value!.Id}", result.Value!), 
            err => Results.BadRequest(err.ToProblemDetails($"/api/attendance/{result.Value!.Id}", "Failed to create attendance"))
        );
    }
}