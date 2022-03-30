using ExchangeRate.Infrastructure.CNB.Core.Repositories;
using ExchangeRate.Infrastructure.CNB.Core.Services;
using ExchangeRate.Infrastructure.Common.Helper;
using Logging.Exceptions;
using Serilog;

namespace ExchangeRate.Infrastructure.CNB.Client.Repositories;

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateRepository(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    public async Task<Core.Models.ExchangeRate> GetExchangeRatesAsync()
    {
        var response = await _exchangeRateService.FetchDataAsync();

        if (response is null)
            throw new HttpRequestException("Http response is null or empty");

        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(content))
            throw new EmptyResultSetException("No content available for CNB exchange rate request");

        return ParseExchangeRateXml(content);
    }

    private static Core.Models.ExchangeRate ParseExchangeRateXml(string content)
    {
        try
        {
            return content.FromXml<Core.Models.ExchangeRate>();
        }
        catch (Exception e)
        {
            Log.Fatal(e, e.Message);
            throw new XmlParsingException("Content from CNB xml request has invalid format");
        }
    }
}
