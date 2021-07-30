#region

using System.Threading.Tasks;
using Reservation.Domain.ReservationRequests;
using Reservation.Infrastructure.Databass.Contexts;

#endregion

namespace Reservation.Infrastructure.Databass.Repositories
{
    public class ReservationRequestRepository : IReservationRequestRepository
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

        public void Update(ReservationRequest reservationRequest)
        {
            _reservationContext.Entry(reservationRequest).CurrentValues.SetValues(reservationRequest);
        }
    }
}