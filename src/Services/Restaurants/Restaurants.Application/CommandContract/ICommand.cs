#region

using MediatR;

#endregion

namespace Restaurants.Application.CommandContract
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}