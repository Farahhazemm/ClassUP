using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace ClassUP.API.Auth
{
    /*
     AuthenticationHandler<MY T OPtions>
     */
    public class BasicAuthHandelercs : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthHandelercs(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
