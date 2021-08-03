#region

using System.Threading.Tasks;
using Restaurants.Infrastructure.Contexts;

#endregion

namespace Restaurants.Infrastructure.Repositories
{
    public class ReservationRequestRepository : IReservationRequestRepository
    {
        private readonly ApplicationContext _applicationContext;

        public ReservationRequestRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task AddAsync(ReservationRequest reservationRequest)
        {
            await _applicationContext.ReservationRequests.AddAsync(reservationRequest);
        }

        public void Update(ReservationRequest reservationRequest)
        {
            _applicationContext.Entry(reservationRequest).CurrentValues.SetValues(reservationRequest);
        }
    }
}