using System;

namespace Reservation.Application
{
    public class DuplicateCommandException:Exception
    {
        public Command Command { get; }

        public DuplicateCommandException(
            Command command, 
            string message)
            :base(message)
        {
            Command = command;
        }
    }
}