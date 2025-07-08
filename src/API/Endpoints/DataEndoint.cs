
using Domain.Entities;
using Infrastructure.Database;

namespace API.Endpoints
{
    public sealed class DataEndoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/data");

            group.MapGet("/{count:int}", FakeData)
                .WithSummary("Generate fake data")
                .WithName(nameof(FakeData))
                .Produces<List<Employee>>();
             

        }

        private static IResult FakeData(int count, DatabaseContext context)
        {
            var dataFaker = new DatabaseFaker();
            var employees = dataFaker.GenerateEmployees(count);

            context.Employees.AddRange(employees);  
            context.SaveChanges();

            return Results.Ok("Create data successful");
        }
    }
}
