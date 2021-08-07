#region

using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Restaurants.Api.Exceptions;
using Restaurants.Application.Contracts;

#endregion

namespace Restaurants.Api.ExecutionContexts
{
    public sealed class WebExecutionContext : IExecutionContext
    {
        private const string CorrelationIdHeader = "X-Correlation-Id";

        public WebExecutionContext(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext is null)
                throw new InvalidOperationException("HttpContext is not available");

            SetCorrelationId(httpContextAccessor);
        }

        public Guid CorrelationId { get; private set; }

        public Guid? CurrentExecutingCommandId { get; set; }

        private void SetCorrelationId(IHttpContextAccessor httpContextAccessor)
        {
            var headers = httpContextAccessor.HttpContext!.Request.Headers;

            if (!headers.TryGetValue(CorrelationIdHeader, out var headerValues))
                throw new CorrelationIdException(
                    $"Request should contains CorrelationId in header '{CorrelationIdHeader}");

            var headerValue = headerValues.First();

            if (!Guid.TryParse(headerValue, out var correlationId))
                throw new CorrelationIdException(
                    $"Provided correlation id '{headerValue}' should be in expected Guid format");

            CorrelationId = correlationId;
        }
    }
}