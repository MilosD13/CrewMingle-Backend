using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Google.Apis.Auth;
using System.Security.Claims;
using FirebaseAdmin.Auth;


namespace CMapi.Authentication;

public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public FirebaseAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Get the Authorization header
        var authHeader = Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return AuthenticateResult.Fail("Missing or invalid Authorization header");
        }

        var idToken = authHeader.Substring("Bearer ".Length);

        try
        {
            // Verify the Firebase ID token
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);

            // Create claims from token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid),
                //new Claim(ClaimTypes.Name, decodedToken.Claims["name"]?.ToString() ?? ""),
                new Claim(ClaimTypes.Email, decodedToken.Claims["email"]?.ToString() ?? "")
            };

            // Create identity and principal
            var identity = new ClaimsIdentity(claims, nameof(FirebaseAuthenticationHandler));
            var principal = new ClaimsPrincipal(identity);

            // Return success
            return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail($"Token validation failed: {ex.Message}");
        }
    }
}

