#region

using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Restaurants.Domain.Tables.ValueObjects;

#endregion

namespace Restaurants.Domain.Tables.DomainRules
{
    public sealed class TableMustHaveAvailableNumberOfSetsRule : IDomainRule
    {
        private readonly NumberOfSeats _actualNumberOfSeats;
        private readonly NumberOfSeats _requestedNumberOfSeats;

        public TableMustHaveAvailableNumberOfSetsRule(
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