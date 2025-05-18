# Exchange Rate Updater API

A .NET API for retrieving currency exchange rates from the Czech National Bank (CNB).

## Features

- Retrieve exchange rates for specific currency pairs
- Support for batch requests of multiple currency pairs
- Date-specific exchange rate data
- Redis caching for improved performance
- Health checks for monitoring API and dependencies
- Containerization with Docker and docker-compose

## Architecture

The solution follows clean architecture principles:

- **Core Layer**: Domain models, interfaces, and business logic
- **Infrastructure Layer**: External integrations (CNB, Redis cache)
- **API Layer**: HTTP endpoints, controllers, and configuration

## Running the API

### Prerequisites

- .NET 9.0 SDK
- Docker & Docker Compose

### Using Docker Compose

```bash
# Clone the repository
git clone <repository-url>
cd ExchangeRateUpdater

# Start the API and Redis
docker-compose up --build
```

The API will be available at:

- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001

### Development Setup

```bash
# Restore packages
dotnet restore

# Run the API
cd src/ExchangeRateUpdater.Api
dotnet run
```

## API Documentation

The API includes Swagger documentation available at the root URL when running in development mode.

### Endpoints

#### Get Exchange Rate

```
GET /api/v1/ExchangeRate?sourceCurrency={sourceCurrency}&targetCurrency={targetCurrency}&date={date}
```

- `sourceCurrency`: Source currency code (e.g., USD)
- `targetCurrency`: Target currency code (e.g., CZK)
- `date` (optional): Date in ISO-8601 format (yyyy-MM-dd)

Example:

```
GET /api/v1/ExchangeRate?sourceCurrency=USD&targetCurrency=CZK&date=2025-05-16
```

Response:

```json
{
  "sourceCurrency": "USD",
  "targetCurrency": "CZK",
  "rate": 22.274,
  "date": "2025-05-16",
  "datePublished": "2025-05-16"
}
```

#### Batch Exchange Rates

```
POST /api/v1/ExchangeRate/batch
```

Request Body:

```json
{
  "date": "2025-05-16",
  "currencyPairs": [
    "USD/CZK",
    "EUR/CZK",
    "GBP/CZK"
  ]
}
```

Response:

```json
{
  "date": "2025-05-16",
  "datePublished": "2025-05-16",
  "rates": [
    {
      "sourceCurrency": "USD",
      "targetCurrency": "CZK",
      "rate": 22.274,
      "date": "2025-05-16",
      "datePublished": "2025-05-16"
    },
    {
      "sourceCurrency": "EUR",
      "targetCurrency": "CZK",
      "rate": 24.930,
      "date": "2025-05-16",
      "datePublished": "2025-05-16"
    },
    {
      "sourceCurrency": "GBP",
      "targetCurrency": "CZK",
      "rate": 29.587,
      "date": "2025-05-16",
      "datePublished": "2025-05-16"
    }
  ]
}
```

## Health Checks

The API includes health check endpoints:

- `/health/live`: Basic application health
- `/health/ready`: Checks dependencies (Redis, CNB API)

## Notes on CNB Data

- Exchange rates of commonly traded currencies are declared every working day after 2:30 p.m.
- Rates are valid for the current working day and, where relevant, the following Saturday, Sunday, or public holiday.
- CNB API access example: https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=16.05.2025
- Date format for the CNB API is dd.MM.yyyy 