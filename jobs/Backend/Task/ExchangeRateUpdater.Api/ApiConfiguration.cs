using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Api
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ApiConfiguration).Assembly);

            return services;
        }
    }
}
