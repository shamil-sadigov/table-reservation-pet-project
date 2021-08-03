#region

using System;
using System.Data.Common;

#endregion

namespace Restaurants.Infrastructure
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