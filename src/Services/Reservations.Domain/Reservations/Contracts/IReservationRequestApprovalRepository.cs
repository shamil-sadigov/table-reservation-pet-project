#region

using System.Threading.Tasks;

#endregion

namespace Reservations.Domain.Reservations.Contracts
{
    public interface IReservationRequestApprovalRepository
    {
        Task AddAsync(Reservation approval);
    }
}