#region

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;
using Reservation.Domain.Tables.DomainRules;

#endregion

namespace Reservation.Domain.Tables
{
    public sealed class NumberOfSeats : ValueObject, IComparable<NumberOfSeats>
    {
        private byte _numberOfSeats;

        private NumberOfSeats(byte numberOfSeats)
        {
            _numberOfSeats = numberOfSeats;
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
            yield return _numberOfSeats;
        }

        public override string ToString()
        {
            return _numberOfSeats.ToString();
        }

        #region Equality operators

        public static bool operator >(NumberOfSeats first, NumberOfSeats second)
            => first._numberOfSeats > second._numberOfSeats;
        
        public static bool operator <(NumberOfSeats first, NumberOfSeats second) 
            => first._numberOfSeats < second._numberOfSeats;

        public static bool operator >=(NumberOfSeats first, NumberOfSeats second) 
            => first._numberOfSeats >= second._numberOfSeats;

        public static bool operator <=(NumberOfSeats first, NumberOfSeats second) 
            => first._numberOfSeats <= second._numberOfSeats;
        
        public static Result<NumberOfSeats> operator -(NumberOfSeats first, NumberOfSeats second)
        {
            var leftNumberOfSeats =  (byte) (first._numberOfSeats - second._numberOfSeats);

            return TryCreate(leftNumberOfSeats);
        }

        #endregion

        public int CompareTo(NumberOfSeats? other)
        {
            if (ReferenceEquals(this, other)) 
                return 0;
            if (ReferenceEquals(null, other)) 
                return 1;
            
            return _numberOfSeats.CompareTo(other._numberOfSeats);
        }
    }
}