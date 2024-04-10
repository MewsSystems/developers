using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Features;
using ExchangeRateUpdater.Presentation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.DependencyInjection
{
    internal class Parser : IParser
    {
        private readonly IServiceProvider _services;

        public Parser(IServiceProvider services)
        {
            _services = services;
        }

        public IOperation Parse(IEnumerable<string> args)
        {
            if (args.Any())
            {
                var first = args.First();
                if (first == "--exchange_rates" || first == "-e")
                {
                    return _services.GetRequiredService<GetExchangeRateOperation>();
                }
                //else if (...)
                //{
                //    return ...
                //}
            }

            // Help command
            return _services.GetRequiredService<GetHelpOperation>();
        }
    }
}
