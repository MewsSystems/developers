using ExchangeRateUpdater.Models.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data.Responses
{
    public class ExchangeRateResultDto
    {
        public ExchangeRateResultDto(Currency sourceCurrency, Currency targetCurrency, decimal value, DateTime date)
        {
            Currencies = $"{sourceCurrency} / {targetCurrency}";
            Value = value;
            Date = date;
        }

        public string Currencies { get; }

        public decimal Value { get; }

        public DateTime Date { get; }
    }
}
