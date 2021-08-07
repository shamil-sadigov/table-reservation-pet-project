using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Restaurants.Api.IntegrationTests
{
    public class TestAuthenticationSchemeOptions:AuthenticationSchemeOptions
    {
        public Claim[] Claims { get; set; }
    }
}