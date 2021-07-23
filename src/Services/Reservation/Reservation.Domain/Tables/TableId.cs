#region

using System;
using System.Collections.Generic;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservation.Domain.Tables
{
    public sealed class TableId : GuidIdentity
    {
        public TableId(Guid id):base(id)
        {
        }
    }
}