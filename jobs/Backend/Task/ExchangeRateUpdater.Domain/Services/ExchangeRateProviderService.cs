namespace ExchangeRateUpdater.Domain.Services;

public class ExchangeRateProviderService : IExchangeRateProviderService
{
    private readonly ILogger<ExchangeRateProviderService> _logger;
    private readonly IValidator<ExchangeRateRequest> _validator;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientWrapper _httpClient;
    private readonly IExchangeRatesParser _exchangeRatesParser;
    private readonly ICalculatorService _calculator;

    public ExchangeRateProviderService(
        ILogger<ExchangeRateProviderService> logger,
        IValidator<ExchangeRateRequest> validator,
        IConfiguration configuration,
        IHttpClientWrapper httpClient,
        IExchangeRatesParser exchangeRatesParser,
         ICalculatorService calculator)
    {
        _logger = logger;
        _validator = validator;
        _configuration = configuration;
        _httpClient = httpClient;
        _exchangeRatesParser = exchangeRatesParser;
        _calculator = calculator;

    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<ExchangeRateResponse> GetExchangeRates(ExchangeRateRequest request)
    {
        var validator = _validator.Validate(request).ToResponse();
        if (!validator.Result)
            return new ExchangeRateResponse { Result = false, Errors = validator.Errors };

        ExchangeRateResponse response = new();
        try
        {
            var exchangeRatesUrl = _configuration.GetSection("ExchangeRatesUrl")?.Value;
            if (string.IsNullOrEmpty(exchangeRatesUrl))
                return ExchangeRatesUrlError();

            using var responseMessage = await _httpClient.GetAsync(exchangeRatesUrl);
            responseMessage.EnsureSuccessStatusCode();
            var responseText = await responseMessage.Content.ReadAsStringAsync();
            var exchangeRates = _exchangeRatesParser.Parse(responseText);
            var existingExchangeRates = exchangeRates.Where(e => request.Currencies.Any(c => c.Code == e.SourceCurrency.Code)).ToList();
            if (existingExchangeRates.Any())
                response.ExchangeRates = _calculator.CalculateRates(existingExchangeRates);
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ExceptionMessages.GetExchangeRatesException, ex);
            response.Result = false;
            response.Errors.Add(new ResponseError { Name = "GetExchangeRatesException", Error = ExceptionMessages.GetExchangeRatesException });
            return response;
        }

        return response;
    }

    private static ExchangeRateResponse ExchangeRatesUrlError()
    {
        return new ExchangeRateResponse
        {
            Result = false,
            Errors = new List<ResponseError>
            {
                new ResponseError
                {
                    Name = "ExchangeRatesUrlError",
                    Error = "Missing ExchangeRatesUrl in the configuration"
                }
            }
        };
    }
}
