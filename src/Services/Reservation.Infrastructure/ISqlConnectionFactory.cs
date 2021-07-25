using System;
using System.Data;

namespace Reservation.Infrastructure.Databass
{
    public interface ISqlConnectionFactory:IDisposable
    {
        IDbConnection GetOrCreateConnection();
    }
}