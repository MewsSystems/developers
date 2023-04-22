﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;
using RestSharp.Authenticators;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider // TODO: Improve with interfaces.
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        private readonly RestClient _client;

        public ExchangeRateProvider()
        {
            _client = new RestClient("https://api.cnb.cz/cnbapi/");
        }
        
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies) //TODO: Make Async
        {
            var request = new RestRequest("exrates/daily");
            var response = _client.ExecuteGet(request);
            if (response.IsSuccessful)
            {
                dynamic document = JsonNode.Parse(response.Content);

                foreach (var rate in document["rates"])
                {
                    yield return new ExchangeRate { CurrencyCode = rate["currencyCode"].ToString(), CurrencyValue = rate["rate"].GetValue<decimal>() };
                }

            }
            else
            {
                throw new ApplicationException($"Error fetching exchange rates: {response.ErrorMessage}");
            }
        }
    }
}
