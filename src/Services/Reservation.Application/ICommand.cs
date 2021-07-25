using System;
using MediatR;

namespace Reservation.Application
{
    public interface ICommand<out TResponse>:IRequest<TResponse>
    {
        Guid CommandId { get; }
    }
}