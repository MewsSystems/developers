# Exchange Rate Updater

A .NET application that provides exchange rates from the Czech National Bank. Available as both a REST API service (with caching) and a command-line application.


### Projects

```
ExchangeRateUpdater.sln
├── ExchangeRateUpdater.Core/          # Shared business logic library
├── ExchangeRateUpdater.Console/       # Console application
├── ExchangeRateUpdater.Api/           # Web API project  
└── ExchangeRateUpdater.Tests/         # Unit and integration tests
```

## Usage Examples

### Console Application

```powershell
# Get specific currencies
dotnet run --project ExchangeRateUpdater.Console -- --currencies USD,EUR,GBP

# Get rates for specific date
dotnet run --project ExchangeRateUpdater.Console -- --currencies USD,EUR,GBP --date 2025-09-28

```

### API Endpoints

```bash
# Get specific currencies
GET http://localhost:5000/api/exchangerates?currencies=USD,EUR

# Combine date and currencies
GET http://localhost:5000/api/exchangerates?date=2025-09-28&currencies=USD,EUR
```

## Docker Setup

1. Build and start the services:
```bash
docker-compose up -d
```

2. Access the applications:
   - API: http://localhost:5000/api/exchangerates
   - Console app: 
     ```bash
     docker-compose run console --date 2025-09-28
     ```

3. Stop the services:
```bash
docker-compose down
```
