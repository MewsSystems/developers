using System;
using System.Collections.Generic;

namespace ExchangeRateUpdaterApi.Dtos.Request;

/// <summary>
/// Dto containing information to request Exchange Rates
/// </summary>
public class ExchangeRatesRequestDto
{
    /// <summary>
    /// Exchange Rates to request
    /// </summary>
    public List<ExchangeRateDetailsDto> ExchangeRatesDetails { get; set; }
    /// <summary>
    /// Date for which ExchangeRates should be returned
    /// </summary>
    public DateTime DateToRequest { get; set; }
}