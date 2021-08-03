#region

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

#endregion

namespace Restaurants.Application.Bahaviors
{
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
            var command = await _commandRepository.GetAsync(request.CommandId);

            if (command is null)
            {
                var newCommand = new ApplicationCommand(
                    request.CommandId,
                    typeof(TRequest).FullName,
                    DateTime.UtcNow);

                await _commandRepository.SaveAsync(newCommand);
            }
            else
            {
                throw new DuplicateCommandException(
                    command,
                    $"Command {command.Id} has been already handled on {command.OccuredOn}");
            }

            return await next();
        }
    }
}