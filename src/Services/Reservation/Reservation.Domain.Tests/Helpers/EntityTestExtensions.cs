#region

using System.Linq;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainEvents;
using FluentAssertions;

#endregion

namespace Reservation.Domain.Tests.Helpers
{
    public static class EntityTestExtensions
    {
        public static T ShouldHavePublishedDomainEvent<T>(this Entity entity)
            where T : IDomainEvent
        {
            var expectedDomainEvent = DomainEventsTestHelper.GetAllDomainEvents(entity)
                .OfType<T>()
                .FirstOrDefault();

            expectedDomainEvent.Should().NotBeNull($"Domain event is expected to be of type {typeof(T)}");

            return expectedDomainEvent!;
        }

        public static T ShouldNotHavePublishedDomainEvent<T>(this Entity entity)
            where T : IDomainEvent
        {
            var publishedDomainEvent = DomainEventsTestHelper.GetAllDomainEvents(entity)
                .OfType<T>()
                .FirstOrDefault();

            publishedDomainEvent.Should().BeNull($"Domain event of type {typeof(T)} is not expected to be published");

            return publishedDomainEvent!;
        }
    }
}