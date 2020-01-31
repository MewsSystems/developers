using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle;
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
				Component.For<ExchangeRateProvider>());
		}
	}
}
