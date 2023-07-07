# Exchange Rate Tool

This is the Exchange Rate Tool library. Created to connect to the CNB api.

## Configuration

In order to use this library, you have to register the library:

```csharp
private static ServiceProvider CreateServiceProvider()
{
    ...
    serviceCollection.AddExchangeRateTool(configuration);
    ...
}
```

This settings should be added to the configuration of the project:

```json
"ServiceClient": {
    "Url": "https://api.cnb.cz/"
},
"Currencies": [
    "USD",
    "EUR",
    "CZK",
    "JPY",
    "KES",
    "RUB",
    "THB",
    "TRY",
    "XYZ"
],
"TargetCurrencyCode": "CZK"
```

- `ServiceClient:Url`: The URL of the Cnb API.
- `Currencies`: Code for all the currencies we want to filter and get the rate, if they exist.
- `TargetCurrencyCode`: The currency's code for the Target currency.

## Usage

In order to use the library, you just have to inject the desired service and call the methods.

Ex:

```csharp

public class MainController
{
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public MainController(IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task GetRates()
    {
        var currencies = ...

        await _exchangeRateProvider.GetExchangeRatesAsync(currencies);
    }
}

```