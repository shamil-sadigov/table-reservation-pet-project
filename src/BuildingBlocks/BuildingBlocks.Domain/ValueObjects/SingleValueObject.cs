#region

using System;
using System.Collections.Generic;

#endregion

namespace BuildingBlocks.Domain.ValueObjects
{
    public class SingleValueObject<TValue> : ValueObject
    {
        protected SingleValueObject(TValue value)
        {
            Value = value ?? throw new ArgumentException(nameof(value));
        }

        public TValue Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString();
    }
}