#region

using System;
using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.Restaurants.DomainRules;
using Reservation.Domain.Tables;

#endregion

namespace Reservation.Domain.Restaurants
{
    public sealed class Restaurant : Entity, IAggregateRoot
    {
        private readonly RestaurantId _id;
        private RestaurantAddress _address;
        private List<Table> _tables;
        private WorkingHours _workingHours;

        private Restaurant(
            WorkingHours workingHours,
            RestaurantAddress address,
            IReadOnlyCollection<NewTableInfo> newTablesInfo)
        {
            _id = new RestaurantId(Guid.NewGuid());
            _workingHours = workingHours;
            _address = address;
            _tables = newTablesInfo.Select(x => new Table(_id, x.TableSize)).ToList();
        }

        public static Result<Restaurant> TryRegisterNew(
            string name,
            WorkingHours workingHours,
            RestaurantAddress address,
            IReadOnlyCollection<NewTableInfo> newTablesInfo)
        {
            if (name.IsNullOrEmpty())
                return Result<Restaurant>.Failure("name should contain value");

            var result = new RestaurantMustHaveAtLeastOneTable(newTablesInfo)
                .Check();

            if (result.Failed)
                return result.WithResponse<Restaurant>(null);

            var restaurant = new Restaurant(workingHours, address, newTablesInfo);

            // TODO: Add domain event

            return Result<Restaurant>.Success(restaurant);
        }
    }
}