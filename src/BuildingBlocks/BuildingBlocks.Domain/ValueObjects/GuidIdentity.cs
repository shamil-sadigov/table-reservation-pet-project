using System;

namespace BuildingBlocks.Domain.ValueObjects
{
    public class GuidIdentity : SingleValueObject<Guid>
    {
        public Guid Value { get; }

        public GuidIdentity(Guid value) : base(value)
        {
            
        }
    }
}