using System.IdentityModel.Tokens.Jwt;
using BlazorApp.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"] ?? throw new InvalidOperationException("Keycloak:Authority is required.");
        options.ClientId = builder.Configuration["Keycloak:ClientId"] ?? throw new InvalidOperationException("Keycloak:ClientId is required.");
        options.ClientSecret = builder.Configuration["Keycloak:ClientSecret"] ?? throw new InvalidOperationException("Keycloak:ClientSecret is required.");
        
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.ResponseType = OpenIdConnectResponseType.Code;

        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;

        options.MapInboundClaims = false;
        options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
        options.TokenValidationParameters.RoleClaimType = "roles";

        options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization();

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp.Client._Imports).Assembly);

app.MapGet("account/login",  async (HttpContext context, string returnUrl = "/") =>
{
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = returnUrl
        };
    
        await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);
});

app.MapGet("account/logout", async (context) =>
{
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = "/"
        };
    
        await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});


app.Run();