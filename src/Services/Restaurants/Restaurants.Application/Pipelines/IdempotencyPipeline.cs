#region

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Restaurants.Application.Contracts;
using Restaurants.Application.Exceptions;

#endregion

namespace Restaurants.Application.Pipelines
{
    // TODO: Add logging
    public class IdempotencyPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IExecutionContext _executionContext;

        public IdempotencyPipeline(
            ICommandRepository commandRepository,
            IExecutionContext executionContext)
        {
            _commandRepository = commandRepository;
            _executionContext = executionContext;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> nextHandler)
        {
            var command = await _commandRepository.GetByCorrelationIdAsync(_executionContext.CorrelationId);

            if (command is not null)
                // TODO: Map this exception to 409 (conflict) on Web layer
                throw new DuplicateRequestException(
                    $"Request with CorrelationId {command.CorrelationId} has been already " +
                    $"sent on {command.CreationDate}");

            var executingCommand = new Command(
                commandId: Guid.NewGuid(),
                _executionContext.CorrelationId,
                causationId: _executionContext.CorrelationId,
                typeof(TRequest).FullName);

            await _commandRepository.SaveAsync(executingCommand);

            _executionContext.CurrentExecutingCommandId = executingCommand.CommandId;

            return await nextHandler();
        }
    }
}