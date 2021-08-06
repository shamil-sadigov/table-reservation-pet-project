using System;

namespace Restaurants.Application
{
    public interface IExecutionContext
    {
        Guid? CurrentExecutingCommandId { get; set; }
        
        // TODO: Set it in Web layer by injecting HttpContextAccessor 
        // and retrievent correlationId
        Guid CorrelationId { get; set; }
    }
}