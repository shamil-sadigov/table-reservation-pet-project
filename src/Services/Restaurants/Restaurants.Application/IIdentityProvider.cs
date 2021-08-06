#region

using System;

#endregion

namespace Restaurants.Application
{
    public interface IIdentityProvider
    {
        Guid UserId { get; }
    }
}