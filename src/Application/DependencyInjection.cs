using Application.Abstraction.Service;
using Application.Service;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            x.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            x.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddHybridCache(x =>
        {
            x.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(1),
                Expiration = TimeSpan.FromMinutes(5),
            };
        });

        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IPayrollService, PayrollService>();

        return services;
    }
}