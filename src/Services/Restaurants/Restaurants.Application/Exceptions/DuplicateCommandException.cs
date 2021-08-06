#region

using System;

#endregion

namespace Restaurants.Application.Exceptions
{
    public class DuplicateCommandException : Exception
    {
        public DuplicateCommandException(
            string message, object command)
            : base(message)
        {
        }
    }
}