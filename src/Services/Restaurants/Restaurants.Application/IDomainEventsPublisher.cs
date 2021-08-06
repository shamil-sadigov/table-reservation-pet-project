#region

using System.Threading.Tasks;

#endregion

namespace Restaurants.Application
{
    public interface IDomainEventsPublisher
    {
        Task PublishEventsAsync();
    }
}