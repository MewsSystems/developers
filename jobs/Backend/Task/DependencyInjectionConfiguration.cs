using System.Collections.Generic;
using ExchangeRateUpdater.CzechNationalBank;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Ninject.Modules;

namespace ExchangeRateUpdater
{
	internal class DependencyInjectionConfiguration : NinjectModule
	{
		public override void Load()
		{
			Bind<IExchangeRateProvider>().To<ExchangeRateProvider>();
			Bind<IDataStringParser<IEnumerable<ExchangeRate>>>().To<CnbExchangeRateDataStringParser>();
			Bind<ILogger>().To<Logger>();
			Bind<IWebRequestService>().To<WebRequestService>();
		}
	}
}
