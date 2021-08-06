#region

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Restaurants.Application.CommandContract;

#endregion

namespace Restaurants.Application.Behaviors
{
    // TODO: Add logging
    public class IdempotencyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IExecutionContext _executionContext;

        public IdempotencyBehavior(
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
            Command? command = await _commandRepository.GetAsync(_executionContext.CorrelationId);

            if (command is not null)
            {
                // TODO: Map this exception to 409 (conflict) on Web layer
                throw new DuplicateCommandException(
                    $"Command {command.CommandId} has been already handled on {command.CreationDate}" +
                    $"with correlation id {command.CorrelationId}", request);
            }
            
            var newCommand = new Command(
                commandId: Guid.NewGuid(), 
                _executionContext.CorrelationId,
                causationId: _executionContext.CorrelationId,
                typeof(TRequest).FullName);

            await _commandRepository.SaveAsync(newCommand);
            return await nextHandler();
        }
    }
}