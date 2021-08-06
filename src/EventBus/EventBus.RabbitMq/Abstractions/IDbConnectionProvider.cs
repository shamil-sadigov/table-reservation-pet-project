#region

using System.Data.Common;

#endregion

namespace EventBus.RabbitMq.Abstractions
{
    public interface IDbConnectionProvider
    {
        DbConnection GetDbConnection();
    }
}