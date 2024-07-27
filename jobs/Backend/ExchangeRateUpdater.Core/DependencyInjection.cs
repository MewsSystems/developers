using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Core.ServiceContracts;
using ExchangeRateUpdater.Core.Services;
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

            services.AddScoped<IExchangeRateGetService, ExchangeRatesGetService>();
            return services;
        }
    }
}
