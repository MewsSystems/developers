namespace ExchangeRateUpdater.Domain.Common
{
    /// <summary>
    /// Value object base class that has basic utility methods like equality based on the comparison
    /// between all the attributes and other fundamental characteristics.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects">
    /// Value objects
    /// </see>
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Checks if two value objects are equal.
        /// </summary>
        /// <param name="left">The left value object to compare.</param>
        /// <param name="right">The right value object to compare.</param>
        /// <returns>True if the value objects are equal, otherwise false.</returns>
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        /// <summary>
        /// Checks if two value objects are not equal.
        /// </summary>
        /// <param name="left">The left value object to compare.</param>
        /// <param name="right">The right value object to compare.</param>
        /// <returns>True if the value objects are not equal, otherwise false.</returns>
        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        /// <summary>
        /// Retrieves the equality components of the value object.
        /// </summary>
        /// <returns>An enumerable collection of the value object's equality components.</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hash = new HashCode();

            foreach (var component in GetEqualityComponents())
            {
                hash.Add(component);
            }

            return hash.ToHashCode();
        }

        /// <summary>
        /// Overloaded equality operator to check if two value objects are equal.
        /// </summary>
        public static bool operator ==(ValueObject one, ValueObject two)
        {
            return EqualOperator(one, two);
        }

        /// <summary>
        /// Overloaded inequality operator to check if two value objects are not equal.
        /// </summary>
        public static bool operator !=(ValueObject one, ValueObject two)
        {
            return NotEqualOperator(one, two);
        }
    }
}
