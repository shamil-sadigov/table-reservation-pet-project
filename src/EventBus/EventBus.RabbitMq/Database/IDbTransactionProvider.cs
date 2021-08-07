using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventBus.RabbitMq.Database
{
    public interface IDbTransactionProvider
    {
        DbTransaction GetCurrentDbTransaction();
    }
}