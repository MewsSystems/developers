using System;

namespace ExchangeRateUpdater.Domain.Base
{
    public abstract class ValueObject
    {
        public override bool Equals(object obj)
        {
            return obj is ValueObject other && GetType() == other.GetType() &&
                   EqualityComparer<string>.Default.Equals(ToString(), other.ToString());
        }

        public override int GetHashCode()
        {
            return ToString()?.GetHashCode() ?? 0;
        }
    }
}