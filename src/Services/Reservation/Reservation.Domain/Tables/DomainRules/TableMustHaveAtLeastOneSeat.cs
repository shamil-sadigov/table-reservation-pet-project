#region

using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;

#endregion

namespace Reservation.Domain.Tables.DomainRules
{
    // TODO: add 'Rule' ending to all defined rules classes

    public sealed class TableMustHaveAtLeastOneSeat : IDomainRule
    {
        private readonly byte _numberOfSeats;

        public TableMustHaveAtLeastOneSeat(byte numberOfSeats)
        {
            _numberOfSeats = numberOfSeats;
        }

        public Result Check()
        {
            return _numberOfSeats < 1
                ? Result.Failure("numberOfSeats must be at least one")
                : Result.Success();
        }
    }
}