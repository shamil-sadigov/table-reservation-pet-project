#region

using System;
using Microsoft.AspNetCore.Http;
using Restaurants.Api.Exceptions;
using Restaurants.Application.Contracts;

#endregion

namespace Restaurants.Api.ExecutionContexts
{
    public class IdentityProvider : IIdentityProvider
    {
        public IdentityProvider(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext is null)
                throw new InvalidOperationException("HttpContext is not available");

            SetUserId(httpContextAccessor);
        }

        public Guid UserId { get; private set; }

        private void SetUserId(IHttpContextAccessor httpContextAccessor)
        {
            var idClaim = httpContextAccessor.HttpContext!
                .User
                .FindFirst("sub");

            if (idClaim is null)
                throw new UserIdentityException("UserId should be provided in claim type 'sub'");

            var userIdStr = idClaim.Value;

            if (!Guid.TryParse(userIdStr, out var userIdGuid))
                throw new UserIdentityException("UserId should be in Guid format");

            UserId = userIdGuid;
        }
    }
}