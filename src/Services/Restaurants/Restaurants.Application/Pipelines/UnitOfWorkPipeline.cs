#region

using System.Threading;
using System.Threading.Tasks;
using EventBus.Abstractions;
using MediatR;
using Restaurants.Application.CommandContract;
using Restaurants.Application.Contracts;

#endregion

namespace Restaurants.Application.Pipelines
{
    public class UnitOfWorkPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly IDomainEventsPublisher _domainEventsPublisher;
        private readonly IExecutionContext _executionContext;
        private readonly IIntegrationEventsPublisher _integrationEventsPublisher;
        private readonly IResilientTransaction _resilientTransaction;

        public UnitOfWorkPipeline(
            IResilientTransaction resilientTransaction,
            IDomainEventsPublisher domainEventsPublisher,
            IIntegrationEventsPublisher integrationEventsPublisher,
            IExecutionContext executionContext)
        {
            _resilientTransaction = resilientTransaction;
            _domainEventsPublisher = domainEventsPublisher;
            _integrationEventsPublisher = integrationEventsPublisher;
            _executionContext = executionContext;
        }

        // TODO: Add logging
        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> nextHandler)
        {
            var response = await _resilientTransaction.ExecuteAsync(async () =>
            {
                var result = await nextHandler();

                await _domainEventsPublisher.PublishEventsAsync();

                return result;
            });

            // TODO: publishing to event bus may fail.
            // So we need to create a separate service
            // that would pull all 'failed to publish' integration events based on some time interval
            // and would try to publish event again
            
            await _integrationEventsPublisher.PublishEventsAsync(_executionContext.CorrelationId);

            return response;
        }
    }
}