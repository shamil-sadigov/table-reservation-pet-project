using System.Threading.Tasks;

namespace Reservations.Domain.Reservations.Contracts
{
    public interface IReservationRequestApprovalRepository
    {
        Task AddAsync(Reservation approval);
    }
}