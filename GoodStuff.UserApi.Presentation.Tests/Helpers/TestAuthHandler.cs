using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GoodStuff.UserApi.Presentation.Tests.Helpers;

public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    TimeProvider clock)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Create a fake identity with any roles you want
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, "SignUp"),
            new Claim(ClaimTypes.Role, "SignIn"),
            new Claim(ClaimTypes.Role, "Delete")
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}