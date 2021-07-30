#region

using System.Threading.Tasks;

#endregion

namespace Reservation.Domain.ReservationRequests
{
    public interface IReservationRequestRepository
    {
        Task AddAsync(ReservationRequest reservationRequest);
        void Update(ReservationRequest reservationRequest);
    }
}