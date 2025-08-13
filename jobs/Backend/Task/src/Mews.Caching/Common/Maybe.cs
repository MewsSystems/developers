namespace Mews.Caching.Common
{
    public class Maybe<T> : IMaybe<T>
    {
        public static readonly Maybe<T> Nothing = new Maybe<T>();

        public bool HasValue { get; private set; }

        public T Value { get; private set; }

        private Maybe()
        {
            HasValue = false;
            Value = default(T);
        }

        internal Maybe(T value)
        {
            HasValue = !object.Equals(value, null);
            Value = value;
        }

        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }
    }
}
