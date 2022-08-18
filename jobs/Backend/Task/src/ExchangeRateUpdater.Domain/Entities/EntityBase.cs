using System.Collections.Generic;

namespace ExchangeRateUpdater.Domain.Entities
{
    public abstract class EntityBase<T>
    {        
        public abstract T Id { get; }

        protected bool Equals(EntityBase<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Id, other.Id);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EntityBase<T>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Id!);
        }

    }
}