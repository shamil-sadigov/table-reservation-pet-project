﻿using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Restaurants.Api.IntegrationTests
{
    public class TestAuthenticationHandler:AuthenticationHandler<TestAuthenticationSchemeOptions>
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
            var claims = new Claim[]
            {
                new("sub", Options.UserId.ToString()),
                new("scope", Options.ApiScope)
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            var user = new ClaimsPrincipal(claimsIdentity);

            var ticket = new AuthenticationTicket(user, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}