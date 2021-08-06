using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurants.Api.IntegrationTests
{
    public class CorrelationIdProvider : DelegatingHandler
    {
        private readonly Guid _correlationId;

        public CorrelationIdProvider(Guid correlationId)
        {
            _correlationId = correlationId;
        }
        
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-Correlation-Id", _correlationId.ToString());
            return base.SendAsync(request, cancellationToken);
        }
    }
}