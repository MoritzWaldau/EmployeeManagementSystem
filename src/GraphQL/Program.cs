var builder = WebApplication.CreateBuilder(args);

builder.AddGraphQL().AddTypes();

builder.AddServiceDefaults()
    .AddLogging()
    .AddDatabase();

builder.Services
    .AddGraphQLServices(builder.Configuration)
    .AddApplicationServices()
    .AddInfrastructureServices();

var app = builder.Build();

app.MapGraphQL();
app.UseSerilogRequestLogging();

app.RunWithGraphQLCommands(args);
