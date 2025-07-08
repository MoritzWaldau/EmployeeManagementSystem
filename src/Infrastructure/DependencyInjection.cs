using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services) =>
        services.AddServices();
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmployeeRepository<Employee>, EmployeeRepository>();
        services.AddScoped<IPayrollRepository<Payroll>, PayrollRepository>();
        services.AddScoped<IAttendanceRepository<Attendance>, AttendanceRepository>();

        return services;
    }
}