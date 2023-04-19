using ExchangeRateUpdater.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ExchangeRateUpdater.WebApi.Services
{
    public class ExchangeRatesDownloaderFromURL : IExchangeRatesDownloaderFromURL
    {
        private readonly HttpClient _httpClient;

        public ExchangeRatesDownloaderFromURL(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetExchangeRatesRawTextFromURL(string url)
        {

            string exchangesRatesRawText = string.Empty;

            try
            {
                exchangesRatesRawText = await _httpClient.GetStringAsync(url);

            }
            catch (Exception exception)
            {
                throw new Exception($"Request to URL '{url}' did not work", exception);
            }

            return exchangesRatesRawText;
        }
    }
}
