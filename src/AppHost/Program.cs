var builder = DistributedApplication.CreateBuilder(args);

var postgresServer = builder.AddPostgres("postgres", port: 5432);
var employeeDb = postgresServer.AddDatabase("employeedb");

var redis = builder.AddRedis("redis");

var api = builder.AddProject<Projects.API>("api")
    .WithReference(employeeDb)
    .WithReference(redis)
    .WaitFor(employeeDb)
    .WaitFor(redis);

var graphql = builder.AddProject<Projects.GraphQL>("graphql")
    .WithReference(employeeDb)
    .WithReference(redis)
    .WaitFor(employeeDb)
    .WaitFor(redis);

builder.AddProject<Projects.BlazorApp>("web")
    .WithReference(api)
    .WithReference(graphql)
    .WithExternalHttpEndpoints()
    .WaitFor(api)
    .WaitFor(graphql);

builder.Build().Run();