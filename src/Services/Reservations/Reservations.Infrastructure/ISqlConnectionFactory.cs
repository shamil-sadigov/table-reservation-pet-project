#region

using System;
using System.Data;

#endregion

namespace Reservations.Infrastructure
{
    public interface ISqlConnectionFactory : IAsyncDisposable, IDisposable
    {
        IDbConnection GetOrCreateConnection();
    }
}