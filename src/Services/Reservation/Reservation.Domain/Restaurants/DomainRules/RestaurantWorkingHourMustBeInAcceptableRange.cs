#region

using System;
using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.Restaurants.ValueObjects;

#endregion

namespace Reservation.Domain.Restaurants.DomainRules
{
    internal sealed class RestaurantWorkingHourMustBeInAcceptableRange : IDomainRule
    {
        private readonly TimeSpan _finishTime;
        private readonly TimeSpan _startTime;

        public RestaurantWorkingHourMustBeInAcceptableRange(TimeSpan startTime, TimeSpan finishTime)
        {
            _startTime = startTime;
            _finishTime = finishTime;
        }

        public Result Check()
        {
            var errors = new List<Error>();

            if (_startTime.Days > 0)
                errors.Add($"startTime cannot be greater than {RestaurantWorkingHours.MaxTime}");

            if (_finishTime.Days > 0)
                errors.Add($"finishTime should not be greater that {RestaurantWorkingHours.MaxTime}");

            if (IsInvalidTimeRange(_startTime))
                errors.Add($"startTime should be in range {RestaurantWorkingHours.MinTime:c}-{RestaurantWorkingHours.MaxTime:c}");

            if (IsInvalidTimeRange(_finishTime))
                errors.Add($"finishTime should be in range {RestaurantWorkingHours.MinTime:c}-{RestaurantWorkingHours.MaxTime:c}");

            if (_startTime > _finishTime)
                errors.Add("startTime should not be greater than finishTime");

            return errors.Any()
                ? Result.Failure(errors)
                : Result.Success();
        }

        private static bool IsInvalidTimeRange(TimeSpan startTime)
        {
            return startTime > RestaurantWorkingHours.MaxTime || startTime < RestaurantWorkingHours.MinTime;
        }
    }
}