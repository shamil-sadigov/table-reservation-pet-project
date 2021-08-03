#region

using System;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Restaurants.Domain.Visitors.ValueObjects
{
    public sealed class VisitorId : GuidIdentity
    {
        public VisitorId(Guid id) : base(id)
        {
        }
    }
}