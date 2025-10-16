var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres", port: 5432);
var employeeDb = postgres.AddDatabase("employeedb");
var keycloakDb = postgres.AddDatabase("keycloakdb");

var redis = builder.AddRedis("redis");

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithDataVolume()
    .WithReference(keycloakDb)
    .WaitFor(keycloakDb);

var api = builder.AddProject<Projects.API>("api")
    .WithReference(employeeDb)
    .WithReference(redis)
    .WithReference(keycloak)
    .WaitFor(employeeDb)
    .WaitFor(redis)
    .WaitFor(keycloak);

var graphql = builder.AddProject<Projects.GraphQL>("graphql")
    .WithReference(employeeDb)
    .WithReference(redis)
    .WithReference(keycloak)
    .WaitFor(employeeDb)
    .WaitFor(redis)
    .WaitFor(keycloak);

builder.AddProject<Projects.BlazorApp>("web")
    .WithReference(api)
    .WithReference(graphql)
    .WithReference(keycloak)
    .WithExternalHttpEndpoints()
    .WaitFor(api)
    .WaitFor(graphql)
    .WaitFor(keycloak);

builder.Build().Run();