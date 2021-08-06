#region

using System.Threading.Tasks;
using MediatR;

#endregion

namespace Restaurants.Application
{
    public class DomainEventsPublisher : IDomainEventsPublisher
    {
        private readonly IDomainEventAccessor _domainEventAccessor;
        private readonly IMediator _mediator;

        public DomainEventsPublisher(IDomainEventAccessor domainEventAccessor, IMediator mediator)
        {
            _domainEventAccessor = domainEventAccessor;
            _mediator = mediator;
        }

        public async Task PublishEventsAsync()
        {
            foreach (var @event in _domainEventAccessor.GetAllDomainEvents())
                await _mediator.Publish(@event);

            _domainEventAccessor.ClearAllDomainEvents();
        }
    }
}