# Exchange Rate Updater — Czech National Bank

A .NET 6.0 application that fetches and provides current exchange rates from the Czech National Bank (CNB) public data source.

## Overview

This project implements an `ExchangeRateProvider` that retrieves daily exchange rates from the CNB and returns only those rates that are defined by the source (foreign currency to Czech Koruna). The implementation strictly adheres to the requirement of not returning calculated or reciprocal exchange rates.

## How It Works

### Data Source

The provider retrieves exchange rates from the Czech National Bank's daily exchange rate fix:

**Primary URLs:**
- Text: `https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt`

**Fallback URLs (Czech interface):**
- Text: `https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/denni.txt`

### Filtering Logic

The provider only returns exchange rates that meet **all** of these criteria:

1. The currency code exists in the CNB source data
2. The currency code was requested by the caller
3. CZK (Czech Koruna) was also requested by the caller
4. The rate is returned in the direction provided by the source (foreign → CZK)

**Example:**
- If CNB provides `USD → CZK = 20.977`, the provider returns exactly that
- The provider does **not** calculate and return `CZK → USD = 0.0477...`
- If USD is requested but CZK is not, no rate is returned

### Health Check

A `/health` endpoint is available to monitor the application's status.

## Building and Running

### Prerequisites

- .NET 6.0 SDK or later
- Windows PowerShell or compatible shell

### Build
cd e:\Projects\developers\jobs\Backend\Task
dotnet build ExchangeRateUpdater.sln -c Debug
### Run the Test Program
cd e:\Projects\developers\jobs\Backend\Task
dotnet run --project ExchangeRateUpdater.csproj

### Example Test Currencies

The default test program (`Program.cs`) requests rates for:
- USD, EUR, JPY, THB, TRY, KES, RUB, CZK, XYZ

Only rates that CNB provides for these currencies (paired with CZK) are returned.

## API Usage

### Get Exchange Rates

**Endpoint:**GET /api/exchangerates?currencies=USD,EUR,CZK
**Query Parameters:**
- `currencies`: Comma-separated ISO 4217 currency codes (e.g., USD, EUR, CZK)

**Response:**[
  { "source": "USD", "target": "CZK", "value": 20.977 },
  { "source": "EUR", "target": "CZK", "value": 24.285 }
]
**Swagger UI:**
- Available at `/swagger` for interactive API documentation and testing.

## Running the Web API

- Build and run as usual. The browser will open Swagger UI automatically in development mode.
- Use the `/api/exchangerates` endpoint to fetch rates for requested currencies.

## Related Files

- `Currency.cs`: Currency model
- `ExchangeRate.cs`: Exchange rate model
- `Program.cs`: CLI test program
- `ExchangeRateUpdater.sln`: Solution file
- `ExchangeRateUpdater.csproj`: Project file