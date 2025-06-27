namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddCarter();
        services.AddHealthChecks();
        services.AddEndpointsApiExplorer();
        services.AddExceptionHandler<ExceptionMiddleware>();
        services.AddHttpContextAccessor();
        services.AddSwagger();
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddProblemDetails();
        return services;
    }
    
    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee Management System API"});
        });
    }


    public static void UseApiServices(this WebApplication app)
        => app
            .UseSwaggerServices()
            .UseServices();

    public static void AddLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(context.Configuration);
        });
    }
    
    private static WebApplication UseSwaggerServices(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    } 
    
    private static void UseServices(this WebApplication app)
    {
        app.MapCarter();
        app.UseHealthChecks("/health");
        app.UseSerilogRequestLogging();
        app.UseExceptionHandler(x => { });
        app.UseHttpsRedirection();
    }
}