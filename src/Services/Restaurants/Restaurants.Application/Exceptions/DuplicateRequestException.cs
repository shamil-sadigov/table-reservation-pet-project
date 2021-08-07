#region

using System;

#endregion

namespace Restaurants.Application.Exceptions
{
    public class DuplicateRequestException : Exception
    {
        public DuplicateRequestException(
            string message)
            : base(message)
        {
        }
    }
}