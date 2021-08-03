using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Restaurants.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            // Add logging here
            return await next();
            // Add logging here
        }
    }
}