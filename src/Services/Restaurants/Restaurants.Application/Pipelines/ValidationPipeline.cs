#region

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

#endregion

namespace Restaurants.Application.Pipelines
{
    public class ValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly ILogger<ValidationPipeline<TRequest, TResponse>> _logger;
        private readonly IValidator<TRequest>[] _validators;

        public ValidationPipeline(
            IValidator<TRequest>[] validators,
            ILogger<ValidationPipeline<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        // TODO: Add logging
        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any()) throw new ValidationException(failures);

            return await next();
        }
    }
}