#region

using System;

#endregion

namespace Restaurants.Application
{
    public class DuplicateCommandException : Exception
    {
        public DuplicateCommandException(
            ApplicationCommand applicationCommand,
            string message)
            : base(message)
        {
            ApplicationCommand = applicationCommand;
        }

        public ApplicationCommand ApplicationCommand { get; }
    }
}