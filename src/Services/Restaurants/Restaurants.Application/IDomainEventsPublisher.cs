using System.Threading.Tasks;

namespace Restaurants.Application
{
    public interface IDomainEventsPublisher
    {
        Task PublishEventsAsync();
    }
}