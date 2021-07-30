#region

using System;
using System.Data;

#endregion

namespace Reservation.Infrastructure.Databass
{
    public interface ISqlConnectionFactory : IDisposable
    {
        IDbConnection GetOrCreateConnection();
    }
}