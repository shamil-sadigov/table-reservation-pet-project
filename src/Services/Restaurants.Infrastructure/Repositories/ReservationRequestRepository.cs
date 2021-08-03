#region

using System.Threading.Tasks;
using Restaurants.Infrastructure.Contexts;

#endregion

namespace Restaurants.Infrastructure.Repositories
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