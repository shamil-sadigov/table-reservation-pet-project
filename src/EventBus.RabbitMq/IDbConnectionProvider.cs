using System.Data.Common;

namespace EventBus.RabbitMq
{
    // Will be implemented in Application layer
    public interface IDbConnectionProvider
    {
        DbConnection GetDbConnection();
    }
}