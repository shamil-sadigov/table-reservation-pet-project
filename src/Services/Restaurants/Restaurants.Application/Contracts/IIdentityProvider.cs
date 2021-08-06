#region

using System;

#endregion

namespace Restaurants.Application.Contracts
{
    public interface IIdentityProvider
    {
        Guid UserId { get; }
    }
}