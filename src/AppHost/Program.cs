

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres", port: 5432);

var redis = builder.AddRedis("redis");

builder.AddProject<Projects.API>("api")
    .WithExternalHttpEndpoints()
    .WithReference(postgres)
    .WithReference(redis)
    .WaitFor(postgres)
    .WaitFor(redis);

builder.Build().Run();