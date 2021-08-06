#region

using System;
using Restaurants.Application.CommandContract;

#endregion

namespace Restaurants.Application
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