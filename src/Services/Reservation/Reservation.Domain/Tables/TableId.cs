#region

using System;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservation.Domain.Tables
{
    public sealed class TableId : ValueObject
    {
        private readonly Guid _id;

        public TableId(Guid id)
        {
            _id = id;
        }
    }
}