#region

using System;
using Restaurants.Application.Commands;

#endregion

namespace Restaurants.Application
{
    public class DuplicateCommandException<TCommand> : Exception
    where TCommand:ICommandBase
    {
        public DuplicateCommandException(
            ICommandBase command,
            string message)
            : base(message)
        {
            Command = command;
        }

        public ICommandBase Command { get; }
    }
}