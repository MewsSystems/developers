namespace ExchangeRateService
{
    using CurrencyExchangeService.Interfaces;
    using Logger;

    public class CNBExchangeRateProvider : IExchangeRateProvider<string>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        
        public CNBExchangeRateProvider(ILogger logger, HttpClient httpClient = null)
        {
            this._logger = logger;
            this._httpClient = httpClient != null ? httpClient : new HttpClient();
        }


        public async Task<string> GetExchangeRates()
        {
            var responseBody = String.Empty;

            try
            {
                // Could be moved to separate service, but for one get call dont want to over complicate it
                // Used constructor with CancellationToken to, because its only one the have ovveride, to be able to mock it
                // In real like project wont use such http client (with out interface)
                HttpResponseMessage response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, CurrencyExchangeService.Constants.CNBExchangeRateApiUrl),
                    new CancellationTokenSource().Token);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();

                this._logger.Info($"CNBExchangeRateProvider:GetExchangeRates: Request to { CurrencyExchangeService.Constants.CNBExchangeRateApiUrl } executed with responce { response }");
            }
            catch (Exception ex)
            {
                this._logger.Error($"CNBExchangeRateProvider:GetExchangeRates: Request to { CurrencyExchangeService.Constants.CNBExchangeRateApiUrl } executed with error { ex.Message }");
            }

            return responseBody;
        }
    }
}
