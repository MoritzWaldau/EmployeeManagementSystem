# OpenID Connect Authentication - Konfigurationsanleitung

## Übersicht
OpenID Connect wurde erfolgreich zum BlazorApp-Projekt hinzugefügt.

## Konfiguration

### 1. appsettings.json anpassen

Öffnen Sie `appsettings.json` und passen Sie die OpenID Connect-Einstellungen an:

#### Für generische OpenID Connect Provider:
```json
"OpenIdConnect": {
  "Authority": "https://your-identity-provider.com",
  "ClientId": "your-client-id",
  "ClientSecret": "your-client-secret",
  "ResponseType": "code",
  "SaveTokens": true,
  "GetClaimsFromUserInfoEndpoint": true,
  "Scope": ["openid", "profile", "email"]
}
```

#### Für Azure AD / Entra ID:
```json
"AzureAd": {
  "Instance": "https://login.microsoftonline.com/",
  "Domain": "your-domain.onmicrosoft.com",
  "TenantId": "your-tenant-id",
  "ClientId": "your-client-id",
  "ClientSecret": "your-client-secret",
  "CallbackPath": "/signin-oidc",
  "SignedOutCallbackPath": "/signout-callback-oidc"
}
```

### 2. Azure AD verwenden (optional)

Wenn Sie Azure AD verwenden möchten, öffnen Sie `DependencyInjection.cs` und kommentieren Sie diese Zeile ein:

```csharp
services.AddMicrosoftIdentityWebAppAuthentication(configuration, "AzureAd");
```

Und kommentieren Sie die generische OpenID Connect-Konfiguration aus.

### 3. Umgebungsvariablen (Empfohlen)

Speichern Sie Secrets NICHT in appsettings.json. Verwenden Sie stattdessen:

#### User Secrets (Entwicklung):
```bash
dotnet user-secrets set "OpenIdConnect:ClientSecret" "your-secret"
dotnet user-secrets set "OpenIdConnect:Authority" "https://your-provider.com"
```

#### Umgebungsvariablen (Produktion):
```
OpenIdConnect__ClientSecret=your-secret
OpenIdConnect__Authority=https://your-provider.com
```

### 4. NuGet-Pakete wiederherstellen

```bash
dotnet restore
```

## Verwendung

### Seiten schützen

Fügen Sie `@attribute [Authorize]` zu Ihren Razor-Seiten hinzu:

```razor
@page "/protected"
@attribute [Authorize]

<h3>Diese Seite ist geschützt</h3>
```

### Rollenbasierte Autorisierung

```razor
@attribute [Authorize(Roles = "Admin")]
```

### Login-Display in Layout einbinden

Fügen Sie die `LoginDisplay` Komponente zu Ihrem Layout hinzu (z.B. MainLayout.razor):

```razor
<LoginDisplay />
```

### Authentifizierungsstatus prüfen

```razor
<AuthorizeView>
    <Authorized>
        <p>Hallo @context.User.Identity?.Name!</p>
    </Authorized>
    <NotAuthorized>
        <p>Sie sind nicht angemeldet.</p>
    </NotAuthorized>
</AuthorizeView>
```

## Routen

- `/login` - Login-Seite
- `/logout` - Logout-Seite
- `/accessdenied` - Zugriff verweigert Seite

## Beliebte OpenID Connect Provider

### Keycloak
```json
"Authority": "https://your-keycloak-server/realms/your-realm"
```

### Auth0
```json
"Authority": "https://your-tenant.auth0.com"
```

### IdentityServer
```json
"Authority": "https://your-identityserver.com"
```

### Okta
```json
"Authority": "https://your-domain.okta.com/oauth2/default"
```

## Troubleshooting

### Redirect URI konfigurieren
Stellen Sie sicher, dass Sie folgende Redirect URIs in Ihrem Identity Provider registrieren:
- `https://localhost:5001/signin-oidc`
- `https://localhost:5001/signout-callback-oidc`
- Ihre Produktions-URLs

### HTTPS erforderlich
OpenID Connect erfordert HTTPS. In der Entwicklung verwenden Sie das selbstsignierte Zertifikat:
```bash
dotnet dev-certs https --trust
```

## Weitere Informationen

- [Microsoft Identity Web Dokumentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/microsoft-identity-web)
- [OpenID Connect Spezifikation](https://openid.net/connect/)
- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)

