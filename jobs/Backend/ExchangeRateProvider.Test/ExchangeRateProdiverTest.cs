using System.Threading.Tasks;
using CurrencyRateUpdater;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Service;
using Moq;

namespace ExchangeRateProvider.Test;

public class ExchangeRateProdiverTest
{
	IEnumerable<Currency> currencies = new[]
	{
		new Currency("USD"),
		new Currency("EUR"),
		new Currency("CZK"),
		new Currency("JPY"),
		new Currency("KES"),
		new Currency("RUB"),
		new Currency("THB"),
		new Currency("TRY"),
		new Currency("XYZ")
	};

	[Fact]
	public async Task Should_Get_A_List_Of_Exchange_Rates()
	{
		var cskCurrency = new Currency("CZK");
		var currencyRatesResponse =
			new CurrencyRatesResponse(
				Rates: new List<CurrencyRateResponse>
				{
					new CurrencyRateResponse
					(
						Currency:"USD Dollar",
						CurrencyCode:"USD",
						Rate:200
					)
				});
		var mockCentralBankService = new Mock<ICentralBankService>();
		mockCentralBankService.Setup(s => s.GetExchangeRateResponse()).ReturnsAsync(currencyRatesResponse);

		IEnumerable<ExchangeRate> exchangeRateExpected = new[]
		{
			new ExchangeRate(
				sourceCurrency: new Currency("USD"),
				targetCurrency: cskCurrency,
				value: 200
			)
		};

		var exchangeRateProvider = new ExchangeRateUpdater.ExchangeRateProvider(mockCentralBankService.Object);
		var result = await exchangeRateProvider.GetExchangeRates(currencies);

		Assert.NotNull(result);
		Assert.NotEmpty(result);
		Assert.Equivalent(exchangeRateExpected, result);
	}
}
