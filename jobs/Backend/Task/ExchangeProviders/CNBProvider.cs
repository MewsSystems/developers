using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Persistence;
using FluentResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeProviders
{
    public class CNBProvider : IExchangeRateProvider
    {
        private readonly string _url;
        private readonly HttpClient _httpClient;
        private readonly IFileProvider _fileProvider;
        private readonly ILogger<CNBProvider> _logger;
        private readonly ICNBParser _cnbParser;

        public CNBProvider(string url, IFileProvider fileProvider, ICNBParser cnbParser, ILogger<CNBProvider> logger)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));
            _url = url;
            _httpClient = new HttpClient();
            _fileProvider = fileProvider;
            _logger = logger;
            _cnbParser = cnbParser;
        }

        public async Task<Result<ExchangeRates>> GetExchangeRates(IEnumerable<Currency> currencies, DateTimeOffset date)
        {
            var response = await _httpClient.GetAsync($"{_url}?date={date.ToString("dd.MM.yyyy")}");

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogWarning($"Couldn't connect to exchange service. Status code is {response.StatusCode}.");

                if (!_fileProvider.TryGetFileContent(out var fileContent))
                {
                    _logger.LogWarning($"Couldn't retrieve data from file. Filename: {fileContent}");
                    return Result.Fail("Couldn't retreive data.");
                }

                return _cnbParser.Parse(fileContent, currencies);
            }

            var content = await response.Content.ReadAsStringAsync();
            if (!_fileProvider.SaveFile(content))
            {
                _logger.LogWarning($"Exchange data weren't saved into file.");
            }

            return _cnbParser.Parse(content, currencies);
        }
    }
}
