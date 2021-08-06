﻿using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.EventBus;
using MediatR;
using Restaurants.Application.CommandContract;

namespace Restaurants.Application.Behaviors
{
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly IResilientTransaction _resilientTransaction;
        private readonly IDomainEventsPublisher _domainEventsPublisher;
        private readonly IIntegrationEventsPublisher _integrationEventsPublisher;
        private readonly IExecutionContext _executionContext;

        public UnitOfWorkBehavior(
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

            await _integrationEventsPublisher.PublishEventsAsync(_executionContext.CorrelationId);
            
            return response;
        }
    }
}