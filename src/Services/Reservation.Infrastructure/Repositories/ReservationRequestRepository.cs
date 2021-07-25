using System.Threading.Tasks;
using Reservation.Domain.ReservationRequests;

namespace Reservation.Infrastructure.Databass.Repositories
{
    public class ReservationRequestRepository:IReservationRequestRepository
    {
        private readonly ReservationContext _reservationContext;

        public ReservationRequestRepository(ReservationContext reservationContext)
        {
            _reservationContext = reservationContext;
        }
        
        public async Task AddAsync(ReservationRequest reservationRequest)
        {
            await _reservationContext.ReservationRequests.AddAsync(reservationRequest);
        }
    }
}