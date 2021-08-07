#region

using System;

#endregion

namespace Restaurants.Application.Contracts
{
    public interface IExecutionContext
    {
        Guid? CurrentExecutingCommandId { get; set; }

        Guid CorrelationId { get; }
    }
}