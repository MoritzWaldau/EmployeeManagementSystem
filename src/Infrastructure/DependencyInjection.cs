namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddDatabaseConnection(configuration)
        .AddServices();
    
    private static IServiceCollection AddDatabaseConnection(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("postgres")
                               ?? throw new DatabaseConnectionException("Database connection string is not configured");

        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(connectionString, builder =>
            {
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        });

        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmployeeRepository<Employee>, EmployeeRepository>();
        services.AddScoped<IPayrollRepository<Payroll>, PayrollRepository>();
        
        return services;
    }
}