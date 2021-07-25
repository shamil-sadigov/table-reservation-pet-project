#region

using System;
using System.Collections.Generic;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservation.Domain.ReservationRequests.ValueObjects
{
    /// <summary>
    /// Time that is specified during creation of reservation request
    /// at which customer is going to visit restaurant
    /// </summary>
    public class VisitingTime : ValueObject
    {
        // For EF
        private VisitingTime()
        {
        }

        private VisitingTime(byte hours, byte minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }

        public byte Hours { get; }
        public byte Minutes { get; }

        public static Result<VisitingTime> TryCreate(byte hours, byte minutes)
        {
            if (hours > 23)
                return new Error("hours cannot not be greater than 23");

            if (minutes > 59)
                return new Error("minutes cannot not be greater than 59");
            
            return new VisitingTime(hours, minutes);
        }

        public TimeSpan AsTimeSpan()
        {
            return new(Hours, Minutes, seconds: 0);
        }

        public override string ToString() => $"{Hours}:{Minutes}";
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Hours;
            yield return Minutes;
        }
    }
}