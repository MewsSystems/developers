using System;
using ExchangeRateUpdater.DataSource.Cnb.Dto;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.DataSource.Cnb;

internal class CnbRateConverter
{
    private readonly ILogger<CnbRateConverter> logger;

    public CnbRateConverter( ILogger<CnbRateConverter> logger)
    {
        this.logger = logger;
    }

    public ExchangeRate Convert(CnbRate rate)
    {
        IsoCurrencyCode sourceCurrency;
        // poor man validation
        if (!Enum.TryParse(rate.CurrencyCode, out sourceCurrency) || rate.Amount <= 0)
        {
            logger.LogWarning("Invalid rate for {} received from CNB, ignoring", rate.CurrencyCode);
            return null;
        }

        return new ExchangeRate(new Currency(sourceCurrency), new Currency(IsoCurrencyCode.CZK), rate.Rate / rate.Amount);
    }
}
