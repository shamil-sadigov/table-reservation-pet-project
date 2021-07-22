#region

using System;
using System.Collections.Generic;
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _id;
        }
    }
}