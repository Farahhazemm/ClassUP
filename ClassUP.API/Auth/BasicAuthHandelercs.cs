using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace ClassUP.API.Auth
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        ) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // 1️ Check Authorization header
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var authHeader = Request.Headers["Authorization"].ToString();

            // 2️ Check "Basic "
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }

            // 3️ Decode Base64
            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();

            string decodedCredentials;
            try
            {
                decodedCredentials = Encoding.UTF8.GetString(
                    Convert.FromBase64String(encodedCredentials)
                );
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Base64"));
            }

            // 4️ Split username:password
            var parts = decodedCredentials.Split(':', 2);
            if (parts.Length != 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Credentials Format"));
            }
            ///////////////////////////////////////////////////////////////

            var username = parts[0];
            var password = parts[1];

            // 5️ Validate user 
            if (username != "admin" || password != "1234")
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
            }

            // 6️ Create Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
