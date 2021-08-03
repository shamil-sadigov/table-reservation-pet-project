#region

using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;
using BuildingBlocks.Helpers;

#endregion

namespace Reservations.Domain.ReservationRequests.ValueObjects
{
    public sealed class TableId : SingleValueObject<string>
    {
        private TableId(string id) : base(id)
        {
        }

        public static Result<TableId> TryCreate(string id)
        {
            if (id.IsNullOrWhiteSpace() || id.Length < 1)
                return new Error("id should not be null");

            return new TableId(id);
        }
    }
}