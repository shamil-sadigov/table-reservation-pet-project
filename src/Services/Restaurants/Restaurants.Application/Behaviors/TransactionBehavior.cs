using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Restaurants.Application.Commands;

namespace Restaurants.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly IResilientTransaction _resilientTransaction;

        public TransactionBehavior(IResilientTransaction resilientTransaction)
        {
            _resilientTransaction = resilientTransaction;
        }
        
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            return await _resilientTransaction.ExecuteAsync(async () => await next());
        }
    }
}