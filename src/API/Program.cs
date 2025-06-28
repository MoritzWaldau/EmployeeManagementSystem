using Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddLogging()
    .AddDatabase();

builder.Services
    .AddApiServices()
    .AddApplicationServices()
    .AddInfrastructureServices();

var app = builder.Build();



app.UseApiServices();
app.Run();
