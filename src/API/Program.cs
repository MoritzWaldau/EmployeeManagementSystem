var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddLogging();

builder.Services
    .AddApiServices()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseApiServices();

app.Run();
