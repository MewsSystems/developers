using ExchangeRateUpdater.Application.Banks;
using ExchangeRateUpdater.Application.ExchangeProvider;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Settings;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Services
{
    public class ExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly IBankFactory bankFactory;
        private readonly AppSettings appSettings;

        public ExchangeRateProviderService(IBankFactory bankFactory, AppSettings appSettings)
        {
            this.bankFactory = bankFactory;
            this.appSettings = appSettings;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var bankConnector = bankFactory.Create(appSettings.ActiveBank);
            var bankRates = await bankConnector.GetExchangeRates(currencies);
            return bankRates;
        }
    }
}
