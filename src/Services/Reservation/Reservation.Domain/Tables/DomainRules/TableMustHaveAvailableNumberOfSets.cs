using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;

namespace Reservation.Domain.Tables.DomainRules
{
    public sealed class TableMustHaveAvailableNumberOfSets:IDomainRule
    {
        private readonly NumberOfSeats _actualNumberOfSeats;
        private readonly NumberOfSeats _requestedNumberOfSeats;

        public TableMustHaveAvailableNumberOfSets(
            NumberOfSeats actualNumberOfSeats, 
            NumberOfSeats requestedNumberOfSeats)
        {
            _actualNumberOfSeats = actualNumberOfSeats;
            _requestedNumberOfSeats = requestedNumberOfSeats;
        }
        
        public Result Check()
        {
            return _actualNumberOfSeats >= _requestedNumberOfSeats
                ? Result.Success()
                : new Error($"actual number of seats '({_actualNumberOfSeats})' " +
                            $"is less than requested number of seats '({_requestedNumberOfSeats})'");
        }
    }
}