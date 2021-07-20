using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.BusinessRule;
using BuildingBlocks.Domain.BusinessRule.AsyncVersion;
using BuildingBlocks.Domain.BusinessRule.SyncVersion;

namespace Reservation.Domain
{
    public class RestaurantWorkingHourMustBeInAcceptableRange:IBusinessRule
    {
        private readonly TimeSpan _startTime;
        private readonly TimeSpan _finishTime;

        public RestaurantWorkingHourMustBeInAcceptableRange(TimeSpan startTime, TimeSpan finishTime)
        {
            _startTime = startTime;
            _finishTime = finishTime;
        }
        
        public Result Check()
        {
            var errors = new List<Error>();
            
            if (_startTime.Days > 0)
                errors.Add(new Error($"startTime cannot be greater than {WorkingHours.MaxTime}"));
            
            if (_finishTime.Days > 0)
                errors.Add(new Error($"finishTime should not be greater that {WorkingHours.MaxTime}"));
            
            if(IsInvalidTimeRange(_startTime))
                errors.Add(new Error($"startTime should be in range {WorkingHours.MinTime:c}-{WorkingHours.MaxTime:c}"));

            if(IsInvalidTimeRange(_finishTime))
                errors.Add(new Error($"finishTime should be in range {WorkingHours.MinTime:c}-{WorkingHours.MaxTime:c}"));

            if (_startTime > _finishTime)
                errors.Add(new Error($"startTime should not be greater than finishTime"));
            
            return errors.Any()
                ? Result.Failure(errors)
                : Result.Success();
        }
        
        private static bool IsInvalidTimeRange(TimeSpan startTime) 
            => startTime > WorkingHours.MaxTime || startTime < WorkingHours.MinTime;
    }
}