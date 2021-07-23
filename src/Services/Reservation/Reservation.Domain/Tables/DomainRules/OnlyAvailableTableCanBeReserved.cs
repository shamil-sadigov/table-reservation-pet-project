using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;

namespace Reservation.Domain.Tables.DomainRules
{
    internal class OnlyAvailableTableCanBeReserved:IDomainRule
    {
        private readonly TableStatus _tableStatus;

        public OnlyAvailableTableCanBeReserved(
            TableId tableId,
            TableStatus tableStatus)
        {
            _tableStatus = tableStatus;
        }
        
        public Result Check()
        {
            return _tableStatus != TableStatus.Available
                ? new Error("Table cannot be reserved because it's not available")
                : Result.Success();
        }
    }
    
    
}