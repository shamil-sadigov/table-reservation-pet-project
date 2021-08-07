#region

using System;

#endregion

namespace Restaurants.Api.Exceptions
{
    public class CorrelationIdException : ApplicationException
    {
        public CorrelationIdException(string message) : base(message)
        {
        }
    }
}