#region

using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Restaurants.Domain.Restaurants.ValueObjects;

#endregion

namespace Restaurants.Domain.Restaurants.DomainRules
{
    internal sealed class RestaurantMustBeOpenAtVisitingTimeRule : IDomainRule
    {
        private readonly RestaurantId _restaurantId;
        private readonly VisitingTime _visitingTime;
        private readonly RestaurantWorkingHours _workingHours;

        public RestaurantMustBeOpenAtVisitingTimeRule(
            RestaurantId restaurantId,
            VisitingTime visitingTime,
            RestaurantWorkingHours workingHours)
        {
            _restaurantId = restaurantId;
            _visitingTime = visitingTime;
            _workingHours = workingHours;
        }

        public Result Check()
        {
            return _workingHours.IsWorkingTime(_visitingTime.AsTimeSpan())
                ? Result.Success()
                : new Error($"Restaurant {_restaurantId} is not open at {_visitingTime} time");
        }
    }
}