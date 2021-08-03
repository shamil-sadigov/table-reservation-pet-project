#region

using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;
using BuildingBlocks.Helpers;

#endregion

namespace Restaurants.Domain.Tables
{
    public sealed class TableId : SingleValueObject<string>
    {
        public TableId(string id) : base(id)
        {
        }

        public static Result<TableId> TryCreate(string id)
        {
            if (id.IsNullOrWhiteSpace()) return new Error("id should not be null");

            return new TableId(id);
        }
    }
}