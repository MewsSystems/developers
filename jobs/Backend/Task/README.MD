# Exchange Rate Provider

This project implements a .NET solution for managing and retrieving echange rates from the Czech Natinal Bank.

# Source for updating daily rates

The daily rates are found at this location:
https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/index.html?date=14.11.2025

And can be accessed directly as a csv plain text file at this location:
https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt

There is also an official official .NET library but is outdated so I went ahead and implemented my own provider with similar idea using csv parsing to split and extract the rates from the csv plain text file.
https://github.com/msigut/CNB.Exchange/tree/master

# Structure 
Split the solution into 5+1 different projects to keep it more organized

src/
- **ExchangeRateUpdater.Api** - Presentation/Application API Layer
- **ExchangeRateUpdate.Client** - This is the test client that uses the API
  **ExchangeRateUpdate.Console** - Console only test of the solution
- **ExchangeRateUpdate.BusinessLogic** - Domain Layer
- **ExchangeRateUpdate.Data** - Infrastructure Layer

test/
- **ExchangeRateUpdate.UniTests** - Unit tests

here is also where integration tests would be added eventually

## Core Models

### `ExchangeRate`
Represents a currency exchange rate with:
- `SourceCurrency`: The currency being converted from
- `TargetCurrency`: The currency being converted to
- `Value`: The exchange rate value

### `Currency`
Represents a currency with its code (e.g., USD, EUR)

## Getting Started

### Prerequisites

- .NET 7.0 or higher
- Visual Studio 2022 or Visual Studio Code

### Observations

#### API Configuration

The API is set to run at `https://localhost:5001` (or as configured in `appsettings.json` and `launchSettings.json`) and has one single GET endpoint 

It also opens a swagger page at `https://localhost:5001/swagger/index.html` for easy testing and exploration of the API.

#### Client Configuration

Currently it doesn't have a setting where to look for the external service in appsettings.json but instead it expecting the API endpoint to be located at localhost:5001 in the javascript. Thus should be changed in the future so that it can be configured from appsettings.json.

## Architecture

The solution follows a layered architecture:

```
┌─────────────────────────────────────┐
│   Presentation Layer                │
│  (Console, API, Client)             │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│   Business Logic Layer              │
│ (ExchangeRateManager, Services)     │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│   Data Access Layer                 │
│ (Repositories, Database, Data)      │
└─────────────────────────────────────┘
```

## License

This is a Mews developer challenge project.

## Contributing

When contributing to this solution:
1. Ensure all unit tests pass
2. Follow C# coding standards
3. Add tests for new functionality
4. Update documentation as needed
