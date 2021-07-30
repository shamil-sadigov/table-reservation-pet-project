#region

using System;
using System.Data.Common;

#endregion

namespace Reservation.Infrastructure.Databass
{
    public class DataCorruptionException : DbException
    {
        public DataCorruptionException(
            string message,
            Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}