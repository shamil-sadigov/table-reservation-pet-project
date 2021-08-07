#region

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

#endregion

namespace Restaurants.Api.IntegrationTests.Auth
{
    public class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public Claim[] Claims { get; set; }
    }
}