﻿using ExchangeRateUpdater.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IApiFetcher
    {
        ApiResponse GetExchangeRates();

    }
}
