var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddLogging()
    .AddDatabase();

builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplicationServices()
    .AddInfrastructureServices();

var app = builder.Build();



app.UseApiServices();
app.Run();

public partial class Program { }
