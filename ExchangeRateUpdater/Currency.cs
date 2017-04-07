using System;

namespace ExchangeRateUpdater
{
    public class Currency
    {

        private string code_;

        public Currency(string code)
        {
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code
        {
            get
            {
                return code_;
            }
            private set
            {
                if (value == null) throw new NullReferenceException("Currency code can't be null.");
                if (value.Length != 3 || value.ToUpper() != value) throw new ArgumentException(value + " is not valid ISO 4217 code of the currency.");
                code_ = value;
            }
        }
    }
}
