using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Models;
//This data should be readonly as its the response from the API
public record DailyRatesDtoContainer(IReadOnlyList<DailyRateDto> Rates);
public record DailyRateDto(
        DateTime ValidFor,
        int Order,
        string Country,
        string Currency,
        int Amount,
        string CurrencyCode,
        decimal Rate
    );
