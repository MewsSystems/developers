using ExchangeEntities;
using ExchangeRateTool.Factories;
using FluentAssertions;
using Moq.AutoMock;
using NUnit.Framework;

namespace ExchangeRateToolTests.Factories
{
	public class ExchangeRateFactoryTests
	{
		private AutoMocker _autoMocker;

		[SetUp]
		public void SetUp()
		{
			_autoMocker = new AutoMocker();
		}

		[Test]
		public void Build_Successfully()
		{
			// Arrange
			string sourceCode = "EUR";
			string targetCode = "CZK";
			decimal value = 31.7m;

			var sut = _autoMocker.CreateInstance<ExchangeRateFactory>();

			// Act
			var result = sut.Build(sourceCode, targetCode, value);

			// Assert
			result.Should().BeOfType<ExchangeRate>();
			result.SourceCurrency.Code.Should().Be(sourceCode);
			result.TargetCurrency.Code.Should().Be(targetCode);
			result.Value.Should().Be(value);
		}
	}
}

