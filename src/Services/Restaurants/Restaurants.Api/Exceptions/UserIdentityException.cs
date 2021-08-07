#region

using System;

#endregion

namespace Restaurants.Api.Exceptions
{
    public class UserIdentityException : ApplicationException
    {
        public UserIdentityException(string message) : base(message)
        {
        }
    }
}