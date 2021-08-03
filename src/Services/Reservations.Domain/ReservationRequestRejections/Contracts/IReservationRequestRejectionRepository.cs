using System.Threading.Tasks;

namespace Reservations.Domain.ReservationRequestRejections.Contracts
{
    public interface IReservationRequestRejectionRepository
    {
        Task AddAsync(ReservationRequestRejection rejection);
    }
}