using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Repositories;

public record DailyRatesResponse(List<RateResource> Rates);
