namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services) =>
        services.AddServices();
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPayrollRepository, PayrollRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        return services;
    }
}