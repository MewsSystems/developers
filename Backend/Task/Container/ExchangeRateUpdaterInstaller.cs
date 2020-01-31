using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater.Container
{
	public class ExchangeRateUpdaterInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<ExchangeRateDownloader>()
						 .DependsOn(Dependency.OnAppSettingsValue("url")),
				Component.For<ExchangeRateProvider>(),
				Component.For<ExchangeRateParser>()
						 .DependsOn(Dependency.OnAppSettingsValue("defaultCurrency"),
									Dependency.OnAppSettingsValue("decimalFormatProvider"),
									Dependency.OnAppSettingsValue("lineSeparator"),
									Dependency.OnAppSettingsValue("valueSeparator"),
									Dependency.OnAppSettingsValue("skippedRows"),
									Dependency.OnAppSettingsValue("rateColumnIndex"),
									Dependency.OnAppSettingsValue("quantityColumnIndex"),
									Dependency.OnAppSettingsValue("targetCurrencyColumnIndex"),
									Dependency.OnAppSettingsValue("expectedHeader"))
				);
		}
	}
}
