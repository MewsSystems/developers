using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Models
{
    public class CurrencyValidator
    {
        private readonly HashSet<string> _validIsoCodes;

        public CurrencyValidator(IEnumerable<string> validIsoCodes)
        {
            _validIsoCodes = new HashSet<string>(validIsoCodes ?? throw new ArgumentNullException(nameof(validIsoCodes)));
        }

        public void ValidateCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Currency code cannot be empty or whitespace.", nameof(code));
            }

            if (code.Length != 3)
            {
                throw new ArgumentException("Currency code must be exactly 3 characters long.", nameof(code));
            }

            if (!code.All(char.IsLetter))
            {
                throw new ArgumentException("Currency code must contain only letters.", nameof(code));
            }

            var normalizedCode = code.ToUpperInvariant();
            if (!_validIsoCodes.Contains(normalizedCode))
            {
                throw new ArgumentException($"'{normalizedCode}' is not a valid ISO 4217 currency code.", nameof(code));
            }
        }
    }
} 