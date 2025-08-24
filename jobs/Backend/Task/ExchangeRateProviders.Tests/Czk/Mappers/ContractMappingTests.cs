using ExchangeRateProviders.Czk.Config;
using ExchangeRateProviders.Czk.Mappers;
using ExchangeRateProviders.Czk.Model;
using NUnit.Framework;

namespace ExchangeRateProviders.Tests.Czk.Mappers;

[TestFixture]
public class ContractMappingTests
{
	[Test]
	public void MapToExchangeRate_AmountOne_ReturnsPerUnit()
	{
		// Arrange
		var when = new DateTime(2025,1,2,0,0,0,DateTimeKind.Utc);
		var dto = new CnbApiExchangeRateDto
		{
			CurrencyCode = "USD",
			Amount = 1,
			Rate = 22.50m,
			ValidFor = when
		};

		// Act
		var result = dto.MapToExchangeRate();

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.SourceCurrency.Code, Is.EqualTo("USD"));
			Assert.That(result.TargetCurrency.Code, Is.EqualTo(Constants.ExchangeRateProviderCurrencyCode));
			Assert.That(result.Value, Is.EqualTo(22.50m));
			Assert.That(result.ValidFor, Is.EqualTo(when));
		});
	}

	[Test]
	public void MapToExchangeRate_MultiUnitAmount_Normalizes()
	{
		// Arrange
		var when = DateTime.UtcNow.Date;
		var dto = new CnbApiExchangeRateDto
		{
			CurrencyCode = "JPY",
			Amount = 100,
			Rate = 17.00m,
			ValidFor = when
		};

		// Act
		var result = dto.MapToExchangeRate();

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.Value, Is.EqualTo(0.17m));
			Assert.That(result.ValidFor, Is.EqualTo(when));
		});
	}

	[Test]
	public void MapToExchangeRate_LowerCaseCurrency_UpperCases()
	{
		var when = DateTime.UtcNow;
		var dto = new CnbApiExchangeRateDto
		{
			CurrencyCode = "eur",
			Amount = 1,
			Rate = 24.00m,
			ValidFor = when
		};

		var result = dto.MapToExchangeRate();

		Assert.Multiple(() =>
		{
			Assert.That(result.SourceCurrency.Code, Is.EqualTo("EUR"));
			Assert.That(result.ValidFor, Is.EqualTo(when));
		});
	}

	[Test]
	public void MapToExchangeRates_ValidList_MapsAll()
	{
		// Arrange
		var now = DateTime.UtcNow.Date;
		var source = new List<CnbApiExchangeRateDto>
		{
			new() { CurrencyCode = "USD", Amount = 1, Rate = 22.50m, ValidFor = now },
			new() { CurrencyCode = "EUR", Amount = 2, Rate = 48.00m, ValidFor = now }, // per-unit 24.00
            new() { CurrencyCode = "JPY", Amount = 100, Rate = 17.00m, ValidFor = now } // per-unit 0.17
        };

		// Act
		var result = source.MapToExchangeRates().ToList();

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result, Has.Count.EqualTo(3));
			Assert.That(result.All(r => r.ValidFor == now));
		});
	}
}