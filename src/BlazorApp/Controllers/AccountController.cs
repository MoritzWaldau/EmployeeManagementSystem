using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Controllers;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login(string returnUrl = "/")
    {
        var redirectUrl = Url.Content(returnUrl);
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        var callbackUrl = Url.Page("/", pageHandler: null, values: null, protocol: Request.Scheme);
        return SignOut(
            new AuthenticationProperties { RedirectUri = callbackUrl },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}

