#region

using System;
using System.Collections.Generic;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;
using Reservation.Domain.Restaurants.DomainRules;

#endregion

namespace Reservation.Domain.Restaurants
{
    /// <summary>
    ///     Time period during a day when restaurant is open
    /// </summary>
    public sealed class RestaurantWorkingHours : ValueObject
    {
        public static readonly TimeSpan MaxTime = new(23, 59, 59);
        public static readonly TimeSpan MinTime = new(06, 00, 00);

        private RestaurantWorkingHours()
        {
        }

        private TimeSpan StartTime { get; init; }
        private TimeSpan FinishTime { get; init; }

        public static Result<RestaurantWorkingHours> TryCreate(TimeSpan startTime, TimeSpan finishTime)
        {
            var rule = new RestaurantWorkingHourMustBeInAcceptableRange(startTime, finishTime);

            var result = rule.Check();

            if (!result.Succeeded)
                return result.WithResponse<RestaurantWorkingHours>(null);

            return new RestaurantWorkingHours
            {
                StartTime = startTime,
                FinishTime = finishTime
            };
        }

        public bool IsWorkingTime(TimeSpan timeSpan)
        {
            return timeSpan < FinishTime 
                && timeSpan > StartTime;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartTime;
            yield return FinishTime;
        }
    }
}