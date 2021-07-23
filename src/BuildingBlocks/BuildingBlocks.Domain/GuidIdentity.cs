using System;
using System.Collections.Generic;
using BuildingBlocks.Domain.ValueObjects;

namespace BuildingBlocks.Domain
{
    public class GuidIdentity : ValueObject
    {
        public Guid Value { get; }

        public GuidIdentity(Guid id)
        {
            Value = id;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}