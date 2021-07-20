#region

using BuildingBlocks.Domain.BusinessRule;
using BuildingBlocks.Domain.BusinessRule.SyncVersion;

#endregion

namespace Reservation.Domain.Tables.Rules
{
    // TODO: add 'Rule' ending to all defined rules classes

    public sealed class TableMustHaveAtLeastOneSeat : IBusinessRule
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