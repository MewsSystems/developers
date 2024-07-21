using System.Collections.Generic;

namespace ExchangeRateUpdater.Models;

public sealed record CnbExchangeData(List<CnbExchangeRate> Rates) { }