using System.Globalization;
using BlazorApp.Backend;
using MudBlazor;
using MudBlazor.Services;
using Refit;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace BlazorApp;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        // Controller für Authentication Endpoints
        services.AddControllers();
        
        // Authentication und Authorization konfigurieren
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            configuration.GetSection("OpenIdConnect").Bind(options);
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.RequireHttpsMetadata = false;
            
            // Explizite Callback-Pfade setzen
            options.CallbackPath = "/signin-oidc";
            options.SignedOutCallbackPath = "/signout-callback-oidc";
            
            // Scopes
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");
            
            // Event Handlers für Debugging
            options.Events = new OpenIdConnectEvents
            {
                OnRedirectToIdentityProvider = context =>
                {
                    // Debug: Zeigt die redirect_uri
                    Console.WriteLine($"Redirect URI: {context.ProtocolMessage.RedirectUri}");
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    context.Response.Redirect("/error");
                    context.HandleResponse();
                    return Task.CompletedTask;
                }
            };
        });

        // Alternative: Microsoft Identity Web (für Azure AD)
        // services.AddMicrosoftIdentityWebAppAuthentication(configuration, "AzureAd");
        
        services.AddAuthorization();
        services.AddCascadingAuthenticationState();
        
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