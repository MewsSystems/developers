using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Models
{
    public class CzechNationalBankExchangeRate
    {
        private const int INDEX_AMOUNT = 2;
        private const int INDEX_CODE = 3;
        private const int INDEX_VALUE = 4;

        public bool IsValidExchangeRate { get; private set; }

        public ExchangeRate ExchangeRate { get; private set; }

        public CzechNationalBankExchangeRate(string unparsedLine)
        {
            int amount = 0;
            decimal value = 0;
            string currencyCode = "";

            var parts = unparsedLine.Split('|');
            IsValidExchangeRate = 
                parts.Length == 5 &&
                int.TryParse(parts[INDEX_AMOUNT], out amount) &&
                decimal.TryParse(parts[INDEX_VALUE], out value) &&
                amount > 0 &&
                value > 0 &&
                (currencyCode = parts[INDEX_CODE]).Length == 3;

            if(IsValidExchangeRate)
            {
                this.ExchangeRate = new ExchangeRate(new Currency(currencyCode), new Currency("CZK"), value / amount);
            }
        }

    }
}
