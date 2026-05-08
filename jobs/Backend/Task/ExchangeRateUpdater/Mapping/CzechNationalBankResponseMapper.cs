using System;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Models.Configuration;
using ExchangeRateUpdater.Models.CzechNationalBankResponse;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Mapping;

public class CzechNationalBankResponseMapper : IExchangeRateMapper<RecordedRate>
{
    private const int DESIREDDECIMALPLACE = 3;
    private readonly string _sourceCurrency;

    public CzechNationalBankResponseMapper(IOptions<ExchangeRateConfigurationOptions> options)
    {
        _sourceCurrency = options.Value.SourceCurrency;
    }
    public ExchangeRate MapToExchangeRate(RecordedRate typeToConvert)
    {
        var calculatedRate = Math.Round(typeToConvert.Rate / typeToConvert.Amount, DESIREDDECIMALPLACE);
        return new ExchangeRate(new Currency(_sourceCurrency), new Currency(typeToConvert.CurrencyCode), calculatedRate);
    }
}