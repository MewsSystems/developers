using System;
using System.Collections.Generic;
using System.Text;
using ExchangeRateUpdater.Financial;

namespace OpenExchangeRates
{
    public interface IOpenExchangeRateProviderOptions : IExchangeRateProviderOptions
    {
		AppId AppId { get; }
    }
}
