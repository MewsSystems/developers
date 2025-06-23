using System.IO;
using ExchangeRateUpdater.ConsoleUI;
using ExchangeRateUpdater.Core.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json").Build();

builder.RegisterCore(config);
var app = builder.Build();
await ConsoleExecutor.Default.ExecuteAsync(app.Services);