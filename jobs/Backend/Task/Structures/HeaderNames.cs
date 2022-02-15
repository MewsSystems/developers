using System;

namespace ExchangeRateUpdater.Structures
{
    public class HeaderNames
    {
        public string[] Names { get; }

        public HeaderNames(params string[] names)
        {
            if (names == default || names.Length == 0)
            {
                throw new Exception($"At least one header name must be specified");
            }
            Names = names;
        }

        public override string ToString()
        {
            return string.Join(", ", Names);
        }
    }
}