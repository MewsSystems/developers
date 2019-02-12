using System;

namespace ExchangeRateUpdater
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Code.Equals(((Currency)obj).Code);
        }
        
        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
