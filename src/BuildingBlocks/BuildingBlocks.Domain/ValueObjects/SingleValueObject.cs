using System;
using System.Collections.Generic;

namespace BuildingBlocks.Domain.ValueObjects
{
    public class SingleValueObject<TValue>:ValueObject
    {
        public TValue Value { get; }

        protected SingleValueObject(TValue value)
        {
            Value = value ?? throw new ArgumentException(nameof(value));
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString();
    }
}