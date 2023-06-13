using ExchangeRateUpdater.Tests.Resources;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace ExchangeRateUpdater.Tests
{
	public class CnbParserTests
	{
		private readonly ITestOutputHelper output;

		public CnbParserTests(ITestOutputHelper output)
		{
			this.output = output;
		}
	
		[Fact]
		public void TestToBeDeleted()
		{
			var res = EmbeddedResource.GetResource("ExchangeRates_2023-06-12.txt");
			output.WriteLine(res);
			res.Should().NotBeNullOrEmpty();
		}
	}
}