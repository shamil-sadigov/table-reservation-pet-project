#region

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Restaurants.Application.CommandContract;

#endregion

namespace Restaurants.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> nextHandler)
        {
            // Add logging here
            return await nextHandler();
            // Add logging here
        }
    }
}