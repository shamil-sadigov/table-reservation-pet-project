#region

using System.Threading.Tasks;

#endregion

namespace Restaurants.Application.Contracts
{
    public interface IDomainEventsPublisher
    {
        Task PublishEventsAsync();
    }
}