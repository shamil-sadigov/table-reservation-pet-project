#region

using System.Threading.Tasks;

#endregion

namespace Reservations.Domain.ReservationRequests.Contracts
{
    public interface IReservationRequestRepository
    {
        Task AddAsync(ReservationRequest reservationRequest);
        void Update(ReservationRequest reservationRequest);
    }
}