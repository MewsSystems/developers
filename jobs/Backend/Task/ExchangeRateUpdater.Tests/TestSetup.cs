using Xunit.Abstractions;
using Xunit.Sdk;
using System;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestFramework("ExchangeRateUpdater.Tests.TestSetup", "ExchangeRateUpdater.Tests")]

namespace ExchangeRateUpdater.Tests
{
	public sealed class TestSetup : XunitTestFramework
	{
		public TestSetup(IMessageSink messageSink)
		  : base(messageSink)
		{
			//Environment.SetEnvironmentVariable("BANK_CLIENT_URL", "https://localmock/api/");
			Environment.SetEnvironmentVariable("BANK_CLIENT_URL", "https://api.cnb.cz/cnbapi/");
		}
	}
}