using API.Interfaces;
using API.Models;
using Provider.Settings;
using Newtonsoft.Json;
using NLog;
using Provider.CzechNationalBank.Dtos;
using Provider.ProviderAttribute;

namespace Provider.CzechNationalBank
{
    [ProviderTypeAttribute("CzechNationalBank")]
    public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider, IDisposable
    {
        //API Doc https://api.cnb.cz/cnbapi/swagger-ui.html#/%2Ffxrates
        private readonly HttpClient _httpClient;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ProviderSettings _settings;

        public CzechNationalBankExchangeRateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _settings = ExchangeRateProviderSettingsLoader.Load("CzechNationalBankSettings.json");

            _httpClient.BaseAddress = new(_settings.BaseAddress);
            _httpClient.Timeout = TimeSpan.FromMilliseconds(_settings.HttpClientTimeout);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public async Task<IEnumerable<ExchangeRate>> GetCurrentExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
        {
            var exchangeRates = new List<ExchangeRate>();
            var rates = await FetchExchangeRatesAsync(DateTime.Today, cancellationToken);

            foreach (var currency in currencies)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (rates.TryGetValue(currency.Code, out var rateValue))
                {
                    _logger.Info("Exchange rate for {CurrencyCode} found.", currency.Code);
                    var exchangeRate = new ExchangeRate(new Currency("CZK"), currency, rateValue);
                    exchangeRates.Add(exchangeRate);
                }
                else
                {
                    _logger.Warn("Exchange rate for {CurrencyCode} not found.", currency.Code);
                }
            }

            return exchangeRates;
        }

        private async Task<Dictionary<string, decimal>> FetchExchangeRatesAsync(DateTime date, CancellationToken cancellationToken)
        {
            string url = string.Format(_settings.ExchangeRateEndpoint, date.ToString("yyyy-MM-dd"));

            try
            {
                var response = await _httpClient.GetAsync(url, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.Info("Exchange rates successfully fetched for {Date}.", date);
                    return ParseExchangeRates(data);
                }
                else
                {
                    HandleHttpError(response);
                    return new Dictionary<string, decimal>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.Error(ex, "HTTP request failed for {Url}.", url);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch exchange rates for {Date}.", date);
                throw;
            }
        }

        private static void HandleHttpError(HttpResponseMessage response)
        {
            if (response.RequestMessage?.RequestUri == null)
            {
                _logger.Error(new Exception("Request URI is null"), "Response error.");
                response.EnsureSuccessStatusCode();
                return;
            }

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    _logger.Warn("Bad request (400) when fetching exchange rates from {Url}.", response.RequestMessage.RequestUri);
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    _logger.Warn("Not found (404) when fetching exchange rates from {Url}.", response.RequestMessage.RequestUri);
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    _logger.Error(new Exception("Internal server error"), "Internal server error (500) when fetching exchange rates from {Url}.", response.RequestMessage.RequestUri);
                    break;
                default:
                    _logger.Error(new Exception("Unexpected status code"), "Unexpected status code {StatusCode} when fetching exchange rates from {Url}.", response.StatusCode, response.RequestMessage.RequestUri);
                    break;
            }

            response.EnsureSuccessStatusCode();
        }

        private static Dictionary<string, decimal> ParseExchangeRates(string data)
        {
            try
            {
                var exchangeRateResponse = JsonConvert.DeserializeObject<ExchangeRateResponseDto>(data);

                if (exchangeRateResponse?.Rates is null)
                {
                    throw new Exception("Unexpected JSON structure: 'rates' field not found or empty.");
                }

                var rates = new Dictionary<string, decimal>();
                foreach (var rate in exchangeRateResponse.Rates)
                {
                    if (rate.CurrencyCode is not null && rate.Rate.HasValue)
                    {
                        rates[rate.CurrencyCode] = rate.Rate.Value;
                    }
                    else
                    {
                        _logger.Warn("Invalid rate entry found: {RateEntry}", rate);
                    }
                }

                _logger.Info("Exchange rates successfully parsed.");
                return rates;
            }
            catch (JsonException ex)
            {
                _logger.Error(ex, "JSON parsing failed for exchange rates data: {Data}.", data);
                throw new InvalidOperationException("Failed to parse exchange rates data due to invalid JSON.", ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to parse exchange rates data: {Data}.", data);
                throw;
            }
        }
    }
}