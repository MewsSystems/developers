using ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Infrastructure.ExternalServices.Builders
{
    public class BuildExchangeRates : IBuildExchangeRates
    {
        List<ExchangeRateBank> IBuildExchangeRates.BuildExchangeRates(string fileTxt)
        {
            var lines = fileTxt.Split('\n').Skip(2);
            var rates = new List<ExchangeRateBank>();

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length < 5) continue;
                rates.Add(new ExchangeRateBank
                {
                    Country = parts[0].Trim(),
                    Currency = parts[1].Trim(),
                    Amount = int.Parse(parts[2].Trim()),
                    Code = parts[3].Trim(),
                    Rate = decimal.Parse(parts[4].Trim(), CultureInfo.InvariantCulture)
                });
            }

            return rates;
        }
    }
}
