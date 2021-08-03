#region

using System;
using System.Data;

#endregion

namespace Restaurants.Infrastructure
{
    public interface ISqlConnectionFactory : IDisposable
    {
        IDbConnection GetOrCreateConnection();
    }
}