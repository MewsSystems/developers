﻿using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Core.ServiceContracts.CurrencySource;
using ExchangeRateUpdater.Core.ServiceContracts.ExchangeRate;
using ExchangeRateUpdater.Core.Services.CurrencySource;
using ExchangeRateUpdater.Core.Services.ExchangeRate;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddExchangeRateUpdaterCore(this IServiceCollection services)
        {
            //Exchange Rates
            services.AddScoped<IExchangeRateGetService, ExchangeRatesGetService>();

            //Currency Sources
            services.AddScoped<ICurrencySourceGetService, CurrencySourceGetService>();

            return services;
        }
    }
}
