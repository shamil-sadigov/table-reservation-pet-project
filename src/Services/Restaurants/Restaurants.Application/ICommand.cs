#region

using System;
using MediatR;

#endregion

namespace Restaurants.Application
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
        Guid CommandId { get; }
    }
}