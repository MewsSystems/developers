# Project Descriptions
- **`ExchangeRateUpdater`**: The main library, designed to be pluggable into existing APIs. It includes its own README. 
- **`ExchangeRateUpdater.Tests`**: A unit test project using XUnit and FluentAssertions as testing utilities. 
- **`ExchangeRateUpdater.Api`**: An example API. Run it with `dotnet run`, then go to `http://localhost:5001/swagger` to access the Swagger UI. You can call the `load-rates` endpoint with the example request: 

```json
{
  "targetCurrency": "CZK",
  "sourceCurrencies": [
    "USD",
    "EUR",
    "XYZ",
    "JPY"
  ]
}