

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres", port: 5432);

builder.AddProject<Projects.API>("api")
    .WithReference(postgres)
    .WaitFor(postgres);

builder.Build().Run();