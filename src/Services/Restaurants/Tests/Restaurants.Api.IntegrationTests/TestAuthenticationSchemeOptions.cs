using System;
using Microsoft.AspNetCore.Authentication;

namespace Restaurants.Api.IntegrationTests
{
    public class TestAuthenticationSchemeOptions:AuthenticationSchemeOptions
    {
        public Guid UserId { get; set; }
        public string ApiScope { get; set; }
    }
}