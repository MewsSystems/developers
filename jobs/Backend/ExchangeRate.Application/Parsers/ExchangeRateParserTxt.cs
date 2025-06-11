using ExchangeRate.Application.DTOs;
using ExchangeRate.Application.Parsers.Interfaces;
using System.Globalization;

namespace ExchangeRate.Application.Parsers
{
    public class ExchangeRateParserTxt : IExchangeRateParserTxt
    {
        public List<ExchangeRateBankDTO> Parse(string file)
        {
            var lines = file.Split('\n').Skip(2);
            var rates = new List<ExchangeRateBankDTO>();

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length < 5) continue;
                rates.Add(new ExchangeRateBankDTO
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
