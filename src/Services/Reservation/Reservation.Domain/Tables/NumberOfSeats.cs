#region

using System;
using System.Collections.Generic;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;
using Reservation.Domain.Tables.DomainRules;

#endregion

namespace Reservation.Domain.Tables
{
    public sealed class NumberOfSeats : ValueObject, IComparable<NumberOfSeats>
    {
        private NumberOfSeats(byte value)
        {
            Value = value;
        }

        public byte Value { get; }

        public int CompareTo(NumberOfSeats? other)
        {
            if (ReferenceEquals(this, other))
                return 0;
            if (ReferenceEquals(null, other))
                return 1;

            return Value.CompareTo(other.Value);
        }

        public static Result<NumberOfSeats> TryCreate(byte numberOfSeats)
        {
            var rule = new TableMustHaveAtLeastOneSeat(numberOfSeats);

            var result = rule.Check();

            if (result.Failed)
                return result.WithResponse<NumberOfSeats>(null);

            return new NumberOfSeats(numberOfSeats);
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
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