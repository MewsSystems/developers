using System;
using System.Text.RegularExpressions;
using ExchangeRateUpdater.Utilities.Extensions;

namespace ExchangeRateUpdater.ViewModels
{
    class Currency : IEquatable<Currency>
    {
        private readonly Regex _regex = new Regex(@"^[A-Z]{3}$", RegexOptions.Compiled);
        
        public Currency(string code)
        {
            Guard.ArgumentStringNotEmpty(nameof(code), code);

            // TODO: FluentValidation package could be a better choice

            if (!_regex.IsMatch(code))
            {
                throw new ArgumentException($"Wrong currency code '{code}', must be ISO 4217 compliant");
            }

            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public bool Equals(Currency other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Code == other.Code;
        }

        public override bool Equals(Object other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Equals((Currency)other);
        }

        public override int GetHashCode()
        {
            var hashcode = base.GetHashCode();

            return (hashcode ^ 397) ^ (Code != null ? Code.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Code;
        }
    }
}
