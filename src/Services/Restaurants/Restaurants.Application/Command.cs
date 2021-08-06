#region

using System;
using BuildingBlocks.Domain.DomainRules;

#endregion

namespace Restaurants.Application
{
    // Take a look here => https://railseventstore.org/docs/v2/correlation_causation/
    // to understand what are these ids  about

    public class Command
    {
        // For EF
        private Command()
        {
            
        }
            
        
        public Command(Guid commandId, Guid correlationId, Guid causationId, string commandType)
        {
            CommandId = commandId;
            CorrelationId = correlationId;
            CausationId = causationId;

            CreationDate = SystemClock.DateTimeNow;
            CommandType = commandType;
        }

        public Guid CommandId { get; }
        public Guid CorrelationId { get; }
        public Guid CausationId { get; }

        public DateTime CreationDate { get; }

        public string CommandType { get; }
    }
}