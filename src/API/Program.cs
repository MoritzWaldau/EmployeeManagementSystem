using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddLogging();

builder.Services
    .AddApiServices()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseApiServices();

app.MapGet("/", async (DatabaseContext context) =>
{
    var faker = new DatabaseFaker();
    var employees = await context.Employees.ToListAsync();

    foreach (var employee in employees)
    {
        var payrolls = faker.GeneratePayrolls(12);
        payrolls.ForEach(payroll => payroll.EmployeeId = employee.Id);
        context.Payrolls.AddRange(payrolls);
    }

    await context.SaveChangesAsync();
    
    return Results.Ok("Database seeded with sample data.");
});

app.Run();
