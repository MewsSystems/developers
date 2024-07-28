using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Domain.Entities
{
    public class Currency
    {
        public Currency(string? code)
        {
            if (string.IsNullOrEmpty(code)) 
            { 
                throw new ArgumentNullException(nameof(code)); 
            }
            if (code.Length != 3)
            {
                throw new ArgumentException(nameof(code));
            }

            Code = code.ToUpperInvariant();
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }
    }
}
