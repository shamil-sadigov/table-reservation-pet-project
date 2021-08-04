using System;

namespace Restaurants.Application.Commands
{
    // Take a look here => https://railseventstore.org/docs/v2/correlation_causation/
    // to understand what are these ids  about
    
    public interface ICommandBase
    {
        Guid CommandId { get; }
        Guid CorrelationId { get; }
        Guid CausationId { get; }
    }
}   