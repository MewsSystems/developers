using System;

namespace ExchangeRateUpdater.Structures
{
    public record ValueObject<TValue>(TValue Value) : IComparable<ValueObject<TValue>>
        where TValue : IComparable<TValue>
    {
        public int CompareTo(ValueObject<TValue>? other)
        {
            if (other == null)
                return Int32.MinValue;
        
            return Value.CompareTo(other.Value);
        }
    }
}