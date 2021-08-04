#region

using MediatR;

#endregion

namespace Restaurants.Application.Commands
{
    public interface ICommand<out TResponse> : ICommandBase, IRequest<TResponse>
    {
        
    }
}