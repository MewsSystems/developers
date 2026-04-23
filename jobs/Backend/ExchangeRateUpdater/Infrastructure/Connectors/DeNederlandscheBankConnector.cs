using ExchangeRateUpdater.Application.Banks;
using ExchangeRateUpdater.Domain.Constants;
using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Domain.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Connectors
{
    public class DeNederlandscheBankConnector : IBankConnector
    {
        private readonly HttpClient httpClient;

        public DeNederlandscheBankConnector(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient(BankConstants.DeNederlandscheBank.HttpClientIdentifier);
        }

        public BankIdentifier BankIdentifier => BankIdentifier.DeNederlandscheBank;

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            throw new NotImplementedException();
        }
    }
}
