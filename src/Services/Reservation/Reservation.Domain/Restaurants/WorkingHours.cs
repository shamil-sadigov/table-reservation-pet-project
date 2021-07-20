#region

using System;
using BuildingBlocks.Domain.BusinessRule;
using BuildingBlocks.Domain.ValueObjects;
using Reservation.Domain.Restaurants.Rules;

#endregion

namespace Reservation.Domain.Restaurants
{
    /// <summary>
    ///     Time period during a day when restaurant is open
    /// </summary>
    public sealed class WorkingHours : ValueObject
    {
        public static readonly TimeSpan MaxTime = new(23, 59, 59);
        public static readonly TimeSpan MinTime = new(06, 00, 00);

        private WorkingHours()
        {
        }

        private TimeSpan StartTime { get; init; }
        private TimeSpan FinishTime { get; init; }

        public static Result<WorkingHours> TryCreate(TimeSpan startTime, TimeSpan finishTime)
        {
            var rule = new RestaurantWorkingHourMustBeInAcceptableRange(startTime, finishTime);

            var result = rule.Check();

            if (!result.Succeeded)
                return result.WithResponse<WorkingHours>(null);

            var workingHours = new WorkingHours
            {
                StartTime = startTime,
                FinishTime = finishTime
            };

            return result.WithResponse(workingHours);
        }
    }
}