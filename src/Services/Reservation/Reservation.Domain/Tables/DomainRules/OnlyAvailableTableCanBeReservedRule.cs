using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;

namespace Reservation.Domain.Tables.DomainRules
{
    internal class OnlyAvailableTableCanBeReservedRule:IDomainRule
    {
        private readonly TableState _tableState;

        public OnlyAvailableTableCanBeReservedRule(
            TableId tableId,
            TableState tableState)
        {
            _tableState = tableState;
        }
        
        public Result Check()
        {
            return _tableState != TableState.Available
                ? new Error("Table cannot be reserved because it's not available")
                : Result.Success();
        }
    }
    
    
}