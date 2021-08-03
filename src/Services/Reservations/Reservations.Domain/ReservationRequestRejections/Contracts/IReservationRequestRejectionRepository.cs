#region

using System.Threading.Tasks;

#endregion

namespace Reservations.Domain.ReservationRequestRejections.Contracts
{
    public interface IReservationRequestRejectionRepository
    {
        Task AddAsync(ReservationRequestRejection rejection);
    }
}