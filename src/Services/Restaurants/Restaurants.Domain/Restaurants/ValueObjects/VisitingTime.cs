#region

using System;
using System.Collections.Generic;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Restaurants.Domain.Restaurants.ValueObjects
{
    /// <summary>
    ///     Time that is specified during creation of reservation request
    ///     at which customer is going to visit restaurant
    /// </summary>
    public class VisitingTime : ValueObject
    {
        // For EF
        private VisitingTime()
        {
        }
        
        public VisitingTime(TimeSpan timeSpan)
        {
            // timeSpan can contains Days which should be ignored
            // This is why we recreate new TimeSpan

            Value = new TimeSpan(timeSpan.Hours, timeSpan.Minutes, 0);
        }
        
        public TimeSpan Value { get; }
        
        public override string ToString() => Value.ToString();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator TimeSpan(VisitingTime visitingTime) => visitingTime.Value;
    }
}