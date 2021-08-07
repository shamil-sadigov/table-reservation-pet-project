#region

using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#endregion

namespace Restaurants.Api.IntegrationTests.Auth
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationSchemeOptions>
    {
        public TestAuthenticationHandler(
            IOptionsMonitor<TestAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options,
            logger,
            encoder,
            clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claimsIdentity = new ClaimsIdentity(Options.Claims);

            var user = new ClaimsPrincipal(claimsIdentity);

            var ticket = new AuthenticationTicket(user, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}