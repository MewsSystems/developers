using ExchangeRateUpdater.Console;
using ExchangeRateUpdater.Core.IoC;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.RegisterCore();
await ConsoleExecutor.Default.ExecuteAsync(services.BuildServiceProvider());