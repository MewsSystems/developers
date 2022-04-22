using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IBankClient bankClient;

        public ExchangeRateProvider(IBankClient bankClient)
        {
            this.bankClient = bankClient ?? throw new ArgumentNullException(nameof(bankClient));
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if(!currencies.Any())
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            var exchangeRates = await bankClient.GetExchangeRates();

            var codes = currencies.Select(currency => currency.Code);
            return exchangeRates.Where(exchangeRate => codes.Contains(exchangeRate.SourceCurrency.Code));
        }
    }
}
