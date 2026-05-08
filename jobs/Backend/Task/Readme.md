# Exchange Rate Provider for Czech National Bank.

ASP.NET Core Web API built on .NET 10 that fetches daily exchange rates from the
Czech National Bank and exposes an endpoint for consumers.
I use https://api.cnb.cz/cnbapi/exrates/daily API with default parameters to retrieve rates.

## Architecture

- Minimal API
- Clean separation between:
    - API layer
    - Application layer
    - Infrastructure

## Infrastructure
- Cnb api parameters are stored in appsettings.json
- ILogger from Microsoft.Extensions.Logging is used to log warnings and errors.
- Provided currency codes are compared with CNB currency codes ignoring case.
- AbstractValidator from FluentValidation is used to validate input parameters.

## Tests

- xUnit
- Unit tests for services and api client
- Integration tests using WebApplicationFactory
- External API calls are mocked

## Future improvements

- Add caching for exchange rates
- Implement resilience policies for external API calls
- Add option to change parameters for CNB API
- Improve exception handling
- Write rates to database for persistency and historical data audit

## How to run


### CLI
```bash
dotnet run --project ExchangeRateUpdater.Api
```

### Swagger
Swagger UI is available at `/swagger/index.html` once the application is running.
The exact port is shown in the application startup logs.

### HTTP requests
The file ExchangeRateUpdater.Api.http can be used to send requests
to the API directly from the IDE.

## API

### GET /exchange-rates

Query parameters:
- `currencies`: list of currency codes

Example:
```http
GET /exchange-rates?currencies=usd&currencies=eur