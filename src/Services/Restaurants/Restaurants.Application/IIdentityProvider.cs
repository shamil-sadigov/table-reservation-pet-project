using System;

namespace Restaurants.Application
{
    public interface IIdentityProvider
    {
        Guid UserId { get; }
    }
}