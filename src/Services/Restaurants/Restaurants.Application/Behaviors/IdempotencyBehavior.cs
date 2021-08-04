#region

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Restaurants.Application.Commands;

#endregion

namespace Restaurants.Application.Behaviors
{
    // TODO: Add logging
    public class IdempotencyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly ICommandRepository _commandRepository;

        public IdempotencyBehavior(ICommandRepository commandRepository)
        {
            _commandRepository = commandRepository;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            Command command = await _commandRepository.GetAsync(request.CorrelationId);

            if (command is not null)
            {
                // TODO: Map this exception to 409 (conflict) on Web layer
                throw new DuplicateCommandException<TRequest>(
                    request, 
                    $"Command {command.CommandId} has been already handled on {command.CreationDate}" +
                    $"with correlation id {command.CorrelationId}");
            }
            
            var newCommand = new Command(
                request.CommandId,
                request.CorrelationId,
                request.CausationId,
                typeof(TRequest).FullName);

            await _commandRepository.AddAsync(newCommand);
            return await next();
        }
    }
}