#region

using System;

#endregion

namespace Restaurants.Application
{
    public class DuplicateCommandException : Exception
    {
        public DuplicateCommandException(
            Command command,
            string message)
            : base(message)
        {
            Command = command;
        }

        public Command Command { get; }
    }
}