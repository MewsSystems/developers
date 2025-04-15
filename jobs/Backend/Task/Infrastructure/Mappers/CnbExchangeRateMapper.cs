using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.DTOs;
using ExchangeRateUpdater.Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.Mappers;

public interface IExchangeRateMapper
{
    IEnumerable<ExchangeRate> Map(CnbRateDto cNbRateDto);
}
public class CnbExchangeRateMapper : IExchangeRateMapper
{
    private readonly string _baseCurrency;
    private readonly ILogger<CnbExchangeRateMapper> _logger;

    public CnbExchangeRateMapper(IOptions<CurrencyOptions> baseCurrency, ILogger<CnbExchangeRateMapper> logger)
    {
        _logger = logger;
        _baseCurrency = baseCurrency.Value.BaseCurrency;
    }

    public IEnumerable<ExchangeRate> Map(CnbRateDto cNbRateDto)
    {
        if (cNbRateDto == null)
            throw new ArgumentNullException(nameof(cNbRateDto));

        LogIfExchangeRatesOutdated(cNbRateDto);

       return cNbRateDto.ExchangeRateDtos.Select(dto =>
            new ExchangeRate(
                new Currency(_baseCurrency),
                new Currency(dto.CurrencyCode),
                dto.Rate / dto.Amount
            )
        ).ToList();
    }
    
    private void LogIfExchangeRatesOutdated(CnbRateDto cNbRateDto)
    {
        var today = DateTime.Today;
        var outdatedCurrencies = new List<string>();

        var validForDate = cNbRateDto.ExchangeRateDtos
            .Select(x => x.ValidFor)
            .FirstOrDefault();

        foreach (var dto in cNbRateDto.ExchangeRateDtos)
        {
            if (DateTime.TryParseExact(dto.ValidFor, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var validDate))
            {
                if (validDate.Date < today)
                    outdatedCurrencies.Add(dto.CurrencyCode);
            }
        }

        if (outdatedCurrencies.Any())
        {
            _logger.LogWarning(
                "Exchange rates are from date {ValidForDate}, not today ({Today}). Outdated currencies: {Currencies}",
                validForDate,
                today.ToString("yyyy-MM-dd"),
                string.Join(", ", outdatedCurrencies));
        }
    }
}