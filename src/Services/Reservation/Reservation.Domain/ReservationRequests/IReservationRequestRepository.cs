using System.Threading.Tasks;

namespace Reservation.Domain.ReservationRequests
{
    public interface IReservationRequestRepository
    {
        Task AddAsync(ReservationRequest reservationRequest);
        void Update(ReservationRequest reservationRequest);

        
    }
}