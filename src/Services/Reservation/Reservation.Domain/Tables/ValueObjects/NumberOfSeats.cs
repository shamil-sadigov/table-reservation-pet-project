#region

using System;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;
using Reservation.Domain.Tables.DomainRules;

#endregion

namespace Reservation.Domain.Tables.ValueObjects
{
    public sealed class NumberOfSeats : SingleValueObject<byte>, IComparable<NumberOfSeats>
    {
        private NumberOfSeats(byte value) : base(value)
        {
        }

        public int CompareTo(NumberOfSeats? other)
        {
            if (ReferenceEquals(this, other))
                return 0;

            return ReferenceEquals(null, other) ? 1 : Value.CompareTo(other.Value);
        }

        public static Result<NumberOfSeats> TryCreate(byte numberOfSeats)
        {
            var rule = new TableMustHaveAtLeastOneSeatRule(numberOfSeats);

            var result = rule.Check();

            return result.Succeeded
                ? new NumberOfSeats(numberOfSeats)
                : result.WithoutValue<NumberOfSeats>();
        }

        #region Equality operators

        public static bool operator >(NumberOfSeats first, NumberOfSeats second)
            => first.Value > second.Value;

        public static bool operator <(NumberOfSeats first, NumberOfSeats second)
            => first.Value < second.Value;

        public static bool operator >=(NumberOfSeats first, NumberOfSeats second)
            => first.Value >= second.Value;

        public static bool operator <=(NumberOfSeats first, NumberOfSeats second)
            => first.Value <= second.Value;

        public static Result<NumberOfSeats> operator -(NumberOfSeats first, NumberOfSeats second)
        {
            var leftNumberOfSeats = (byte) (first.Value - second.Value);

            return TryCreate(leftNumberOfSeats);
        }

        #endregion
    }
}