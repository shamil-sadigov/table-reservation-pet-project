#region

using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;

#endregion

namespace Reservation.Domain.Tables.DomainRules
{
    // TODO: add 'Rule' ending to all defined rules classes
    public sealed class TableMustHaveAtLeastOneSeatRule : IDomainRule
    {
        private readonly byte _numberOfSeats;

        public TableMustHaveAtLeastOneSeatRule(byte numberOfSeats)
        {
            _numberOfSeats = numberOfSeats;
        }

        public Result Check()
        {
            // TODO: does it mean that numberOfSeats can be up to 256 ?? No! We should fix it
            return _numberOfSeats < 1
                ? Result.Failure("numberOfSeats must be at least one")
                : Result.Success();
        }
    }
}