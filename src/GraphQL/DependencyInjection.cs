namespace GraphQL;

public static class DependencyInjection
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services, IConfiguration configuration)
    {

        return services;
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(context.Configuration);
        });

        return builder;
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<DatabaseContext>(
            "employeedb",
            configureDbContextOptions: optionsBuilder => optionsBuilder.UseNpgsql(x => x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
        );
    }
}
