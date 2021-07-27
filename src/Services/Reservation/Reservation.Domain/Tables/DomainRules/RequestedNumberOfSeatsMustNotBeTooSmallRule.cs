using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.Tables.ValueObjects;

namespace Reservation.Domain.Tables.DomainRules
{
    internal class RequestedNumberOfSeatsMustNotBeTooSmallRule:IDomainRule
    {
        private readonly TableId _tableId;
        private readonly NumberOfSeats _existingNumberOfSeats;
        private readonly NumberOfSeats _requestedNumberOfSeats;

        public RequestedNumberOfSeatsMustNotBeTooSmallRule(
            TableId tableId,
            NumberOfSeats existingNumberOfSeats, 
            NumberOfSeats requestedNumberOfSeats)
        {
            _tableId = tableId;
            _existingNumberOfSeats = existingNumberOfSeats;
            _requestedNumberOfSeats = requestedNumberOfSeats;
        }
        
        public Result Check()
        {
            if(_existingNumberOfSeats == _requestedNumberOfSeats)
                return Result.Success();
            
            Result<NumberOfSeats> result  
                = _existingNumberOfSeats - _requestedNumberOfSeats;

            if (result.Failed)
                return result;

            var leftNumberOfSeats = result.Value!;
            
            if(leftNumberOfSeats > _requestedNumberOfSeats)
                return Result.Failure($"Table {_tableId} is too big for requested number of seats {_requestedNumberOfSeats}");
            
            return Result.Success();
        }
    }
}