# Exchange Rate Updater Client

This library allows clients to obtain exchange rate data from the Czech National Bank.

### Example

#### Registration

We expose one `ServiceCollection` extension method, `AddExchangeRateProvider`.

##### AddExchangeRateProvider

This method registers an instance of the `CzechNationalBankExchangeRateProvider` as an `IExchangeRateProvider` into the
service collection.

There is 1 parameter required when calling this method - `configurationSection`. This should reference
an `IConfiguration` section with a child element named `ExchangeRateProviders`.

This configuration section should list out the multiple URIs where the Czech National Bank exposes exchange rate data:

source file:

```c#
public static void Main() 
{
    var serviceProvider = new ServiceCollection()
            .AddExchangeRateProvider(configuration.GetRequiredSection(CzechNationalBankExchangeRateProviderOptions.Section))
            .BuildServiceProvider();
            
    using var scope = serviceProvider.CreateScope();
    var rateProvider = scope.ServiceProvider.GetRequiredService<IExchangeRateProvider>();
    var rates = await rateProvider.GetExchangeRatesAsync(new List<Currency>{ new Currency("AUD") }, CancellationToken.None);
}
```

appsettings.json file:

```json
{
  "CzechNationalBankExchangeRateProviderConfiguration": {
    "ExchangeRateProviders": [
      {
        "Uri": "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt"
      },
      {
        "Uri": "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt"
      }
    ]
  }
}
```