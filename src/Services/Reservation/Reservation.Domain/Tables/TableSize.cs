#region

using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;
using Reservation.Domain.Tables.DomainRules;

#endregion

namespace Reservation.Domain.Tables
{
    public sealed class TableSize : ValueObject
    {
        private byte _numberOfSeats;

        private TableSize(byte numberOfSeats)
        {
            _numberOfSeats = numberOfSeats;
        }

        public static Result<TableSize> TryCreate(byte numberOfSeats)
        {
            var rule = new TableMustHaveAtLeastOneSeat(numberOfSeats);

            var result = rule.Check();

            if (result.Failed)
                return result.WithResponse<TableSize>(null);

            var tableSize = new TableSize(numberOfSeats);

            return result.WithResponse(tableSize);
        }
    }
}