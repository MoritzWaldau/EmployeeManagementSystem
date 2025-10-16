using System.Text.Json;
using API.Endpoints;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks();
        services.AddEndpointsApiExplorer();
        services.AddExceptionHandler<ExceptionMiddleware>();
        //services.AddTransient<LoggingMiddleware>();
        services.AddHttpContextAccessor();
        services.AddSwagger(configuration);
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });
        services.AddProblemDetails();
        return services;
    }
    
    private static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = $"Employee Management System API",
            });
        });
    }

    public static void UseApiServices(this WebApplication app)
        => app
            .UseSwaggerServices()
            .UseServices()
            .MapDatabase();

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
    
    private static WebApplication UseSwaggerServices(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    } 
    
    private static WebApplication UseServices(this WebApplication app)
    {
        app.MapEmployeeEndpoints();
        app.MapAttendanceEndpoints();
        app.MapPayrollEndpoints();
        app.MapDataEndpoints();
        app.UseHealthChecks("/health");
        app.UseSerilogRequestLogging();
        //app.UseMiddleware<LoggingMiddleware>();
        app.UseExceptionHandler(x => { });
        app.UseHttpsRedirection();

        return app;
    }

    private static void MapDatabase(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            dbContext.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<DatabaseContext>>();
            logger.LogCritical(ex, "An error occurred creating the DB.");
        }
    }
}