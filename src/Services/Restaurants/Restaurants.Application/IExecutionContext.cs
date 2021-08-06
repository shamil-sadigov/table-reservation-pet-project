#region

using System;

#endregion

namespace Restaurants.Application
{
    public interface IExecutionContext
    {
        Guid? CurrentExecutingCommandId { get; set; }
        
        Guid CorrelationId { get; }
    }
}