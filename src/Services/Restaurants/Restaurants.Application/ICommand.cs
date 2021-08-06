#region

using MediatR;

#endregion

namespace Restaurants.Application
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}