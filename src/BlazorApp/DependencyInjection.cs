using System.Globalization;
using BlazorApp.Backend;
using MudBlazor;
using MudBlazor.Services;
using Refit;

namespace BlazorApp;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        services.AddRefitClient<IApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://api");
            });
        services.ConfigureMudBlazor();
    }
    
    private static void ConfigureMudBlazor(this IServiceCollection services)
    {
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 10000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Outlined;
        });
        services.Configure<PopoverOptions>(options =>
        {
            options.ThrowOnDuplicateProvider = false;
        });
        
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
    }
}