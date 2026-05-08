# Exchange Rate Updater

A .NET application that provides exchange rates from the Czech National Bank. Available as both a REST API service (with caching) and a command-line application.

## Features

- REST API with built-in caching for efficient rate lookups
- Command-line interface for quick rate checks
- Supports multiple currency queries
- Historical exchange rate lookups
- Swagger/OpenAPI documentation
- Docker support for easy deployment
- Telemetry integration
- Global error handling

## Prerequisites

- .NET 9.0 or later
- Docker (optional, for containerized deployment)

## Usage Examples

### Console Application

```powershell
# Below are from the ExchangeRateUpdater.Console folder 
# Get specific currencies
dotnet run --currencies USD,EUR,GBP

# Get rates for specific date
dotnet run --currencies USD,EUR,GBP --date 2025-09-28

```

### API Endpoints

Port will be 5216 when running with .NET locally or 8080 when running via docker.

```bash
# Get specific currencies for the most recent date
GET http://localhost:8080/api/exchangerates?currencies=USD,EUR

# Currencies can also be passed as a list like
GET http://localhost:8080/api/exchangerates?currencies=USD&currencies=EUR&date=2025-09-28

# Combine date and currencies
GET http://localhost:8080/api/exchangerates?date=2025-09-28&currencies=USD,EUR
```

Swagger is available at `http://localhost:8080/swagger`.

## Docker Setup

1. Build and start the services:
```bash
docker-compose up -d
```

2. Access the applications:
   - API: http://localhost:8080/api/exchangerates
   - Console app: 
     ```bash
     docker-compose run console --date 2025-09-28
     ```

3. Stop the services:
```bash
docker-compose down
```
