#region

using System.Data.Common;

#endregion

namespace EventBus.RabbitMq.Database
{
    public interface IDbTransactionProvider
    {
        DbTransaction GetCurrentDbTransaction();
    }
}