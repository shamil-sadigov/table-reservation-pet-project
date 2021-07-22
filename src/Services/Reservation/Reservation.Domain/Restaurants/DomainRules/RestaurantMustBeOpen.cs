#region

using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;

#endregion

namespace Reservation.Domain.Restaurants.DomainRules
{
    internal sealed class RestaurantMustBeOpen : IDomainRule
    {
        private readonly RestaurantWorkingHours _workingHours;

        public RestaurantMustBeOpen(RestaurantWorkingHours workingHours)
        {
            _workingHours = workingHours;
        }


        public Result Check()
        {
            return _workingHours.IsWorkingTime(SystemTime.Now.TimeOfDay)
                ? Result.Success()
                : new Error("Restaurant must be open");
        }
    }
}