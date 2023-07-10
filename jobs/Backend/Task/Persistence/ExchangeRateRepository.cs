using ExchangeRateUpdater.Models.Types;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Persistence;
internal class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly IConfiguration _configuration;

    public ExchangeRateRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<Currency> GetSourceCurrencies()
    {
        return GetSourceCurrenciesFromAppSettings();
    }

    private IEnumerable<Currency> GetSourceCurrenciesFromAppSettings()
    {
        return _configuration
        .GetSection("SourceCurrencies")
        .Get<IEnumerable<string>>()
        .Select(codeValue => new Currency(new Code(codeValue)));
    }
}