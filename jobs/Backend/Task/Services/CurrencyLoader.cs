using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ExchangeRateUpdater.Services
{
    public class CurrencyLoader : ICurrencyLoader
    {
        private readonly IConfiguration _configuration;

        public CurrencyLoader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Currency> LoadCurrencies()
        {
            var jsonFilePath = _configuration["CurrenciesFilePath"];
            var json = File.ReadAllText(jsonFilePath);
            var currencyCodes = JsonSerializer.Deserialize<List<string>>(json);

            return currencyCodes.Select(code => new Currency(code));
        }
    }
}
