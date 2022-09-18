using Castle.DynamicProxy;
using ExchangeRateUpdater;
using FluentAssertions;
using Ninject;

namespace ExchangeRateUpdaterTests
{
	[TestClass]
	[TestCategory("Online")]
	public class OnlineIntegrationTests
	{
		private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

		private IKernel kernel;
		private ILogger logger;
		private Program program;

		[TestInitialize]
		public void TestInitialize()
		{
			kernel = new StandardKernel(new DependencyInjectionConfiguration());
		}

		[TestMethod]
		public async Task ExchangeRateUpdater_SmokeTest()
		{
			var logInfoMessages = new List<string>();

			logger = kernel.Get<ILogger>();

			var logInfoAsyncInterceptor = new MethodInterceptor(nameof(ILogger.LogInfoAsync), invocation =>
			{
				logInfoMessages.Add((string)invocation.Arguments[0]);
			});
			var proxyLogger = proxyGenerator.CreateInterfaceProxyWithTarget(logger, logInfoAsyncInterceptor);

			kernel.Rebind<ILogger>().ToConstant(proxyLogger);
			program = kernel.Get<Program>();

			await program.RunAsync().ConfigureAwait(false);

			logInfoMessages.Should()
				.HaveCountGreaterThan(1);
			logInfoMessages[0].Should()
				.MatchRegex("Successfully retrieved \\d{1,} exchange rates:");
		}
	}
}
