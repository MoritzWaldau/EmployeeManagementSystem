# Keycloak Konfiguration für Blazor App

## Wichtig: Redirect URIs in Keycloak konfigurieren

Der Fehler `Invalid parameter: redirect_uri` tritt auf, weil die Redirect URI in Keycloak nicht registriert ist.

### Schritte in Keycloak:

1. **Keycloak Admin Console öffnen**: `http://localhost:8080`

2. **Zum Client navigieren**:
   - Realm: `shop`
   - Clients → `blazor-client`

3. **Valid Redirect URIs hinzufügen**:
   ```
   https://localhost:*
   https://localhost:*/signin-oidc
   http://localhost:*
   http://localhost:*/signin-oidc
   ```
   
   Oder spezifischer (empfohlen):
   ```
   https://localhost:7154/signin-oidc
   https://localhost:5001/signin-oidc
   http://localhost:5000/signin-oidc
   ```

4. **Valid Post Logout Redirect URIs hinzufügen**:
   ```
   https://localhost:*
   http://localhost:*
   ```

5. **Web Origins hinzufügen**:
   ```
   https://localhost:*
   http://localhost:*
   ```

6. **Client Settings überprüfen**:
   - Client Protocol: `openid-connect`
   - Access Type: `confidential`
   - Standard Flow Enabled: `ON`
   - Direct Access Grants Enabled: `ON` (optional)
   - Valid Redirect URIs: Siehe oben

7. **Speichern** nicht vergessen!

## Ihre aktuelle Konfiguration

- **Authority**: `http://localhost:8080/realms/shop`
- **ClientId**: `blazor-client`
- **CallbackPath**: `/signin-oidc`
- **SignedOutCallbackPath**: `/signout-callback-oidc`

## Die vollständige Redirect URI

Wenn Ihre Blazor App auf `https://localhost:7154` läuft, wird die vollständige Redirect URI sein:
```
https://localhost:7154/signin-oidc
```

## Testen

1. Starten Sie die Blazor App
2. Navigieren Sie zu `/login`
3. Klicken Sie auf "Mit OpenID Connect anmelden"
4. Sie werden zu Keycloak weitergeleitet
5. Nach erfolgreicher Anmeldung werden Sie zurück zur App geleitet

## Troubleshooting

### Port der Anwendung herausfinden:
Schauen Sie in `Properties/launchSettings.json` oder starten Sie die App und prüfen Sie die Console-Ausgabe.

### Debug-Ausgabe:
Die Console zeigt nun die verwendete Redirect URI an:
```
Redirect URI: https://localhost:7154/signin-oidc
```

Kopieren Sie diese URI und fügen Sie sie in Keycloak ein.

