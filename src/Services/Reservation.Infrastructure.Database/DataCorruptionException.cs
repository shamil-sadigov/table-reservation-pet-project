using System;
using System.Data.Common;

namespace Reservation.Infrastructure.Database.Configurations
{
    public class DataCorruptionException:DbException
    {
        public DataCorruptionException(
            string message,
            Exception? innerException = null):base(message, innerException)
        {

        }
    }
}