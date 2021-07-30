#region

using System;

#endregion

namespace BuildingBlocks.Domain.ValueObjects
{
    public class GuidIdentity : SingleValueObject<Guid>
    {
        public GuidIdentity(Guid value) : base(value)
        {
        }
    }
}