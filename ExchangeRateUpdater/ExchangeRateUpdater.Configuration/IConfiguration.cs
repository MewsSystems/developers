using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateUpdater.Configuration
{
    public interface IConfiguration<TTarget>
    {
		TTarget Configure(TTarget target);
    }
}
