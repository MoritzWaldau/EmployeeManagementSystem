var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("postgres", port: 5432)
    .WithPgAdmin()
    .AddDatabase("EmployeeManagementSystem");

builder.AddProject<Projects.API>("api")
    .WithReference(postgres)
    .WaitFor(postgres);

builder.Build().Run();