# Exchange Rate Updater

A comprehensive multi-protocol Exchange Rate API system that fetches, stores, and serves currency exchange rates from multiple national banks. Built using Clean Architecture principles with support for REST, SOAP, and gRPC protocols.

## Overview

This system provides a unified interface to query exchange rates from multiple providers (European Central Bank, Czech National Bank, Romanian National Bank) through three different API protocols. It features automated background jobs for fetching rates, health monitoring, historical data tracking, and real-time updates via SignalR.

## Features

- **Multi-Protocol APIs**
  - REST API with OpenAPI/Swagger documentation
  - SOAP Web Services
  - gRPC with Protocol Buffers

- **Exchange Rate Providers**
  - European Central Bank (ECB) - EUR base
  - Czech National Bank (CNB) - CZK base
  - Romanian National Bank (BNR) - RON base

- **Core Functionality**
  - Real-time and historical exchange rate queries
  - Currency conversion with automatic rate calculation
  - Provider health monitoring and statistics
  - System health dashboard
  - Automated background jobs with Hangfire
  - SignalR real-time updates (REST API)

- **Security & Authentication**
  - JWT-based authentication
  - Role-based authorization (Admin, Consumer)
  - Rate limiting
  - Password policy enforcement

- **Data Management**
  - Configurable data retention policies
  - Automatic cleanup of old data
  - Historical data fetching on startup
  - Provider-specific scheduling

## Architecture

The solution follows Clean Architecture principles with clear separation of concerns:

```
ExchangeRateUpdater/
├── ApiLayer/                    # Presentation Layer
│   ├── REST/                    # REST API (ASP.NET Core)
│   ├── SOAP/                    # SOAP Web Services
│   └── gRPC/                    # gRPC Services
├── ApplicationLayer/            # Application Logic
│   ├── Commands/                # CQRS Command handlers
│   ├── Queries/                 # CQRS Query handlers
│   ├── Behaviors/               # MediatR pipeline behaviors
│   └── DTOs/                    # Data Transfer Objects
├── DomainLayer/                 # Domain Models & Business Rules
├── InfrastructureLayer/         # Infrastructure Services
│   ├── Authentication/          # JWT authentication
│   ├── BackgroundJobs/          # Hangfire job configurations
│   └── Adapters/                # External service adapters
├── DataLayer/                   # Data Access
│   ├── Repositories/            # Repository implementations
│   ├── Configurations/          # EF Core configurations
│   └── Seeding/                 # Database seeders
├── ConfigurationLayer/          # Configuration Management
├── ExchangeRateProviderLayer/   # Provider Implementations
│   ├── EuropeanCentralBank/
│   ├── CzechNationalBank/
│   └── RomanianNationalBank/
├── TestLayer/                   # Testing
│   ├── Unit/                    # Unit tests
│   ├── Integration/             # Integration tests
│   └── ConsoleTestApp/          # Interactive test console
└── Database/                    # SQL Database Project
```

## Prerequisites

- **.NET 9.0 SDK** or later
- **SQL Server LocalDB** (for local development)
- **Visual Studio 2022** or **Rider** (optional)
- **Windows OS** (for LocalDB, or configure for SQL Server on other platforms)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd ExchangeRateUpdater
```

### 2. Database Setup

The application uses SQL Server LocalDB by default. To initialize:

```bash
# Start LocalDB
sqllocaldb start MSSQLLocalDB

# Database will be created automatically on first run
```

**Alternative:** Use in-memory database by setting `Database:UseInMemoryDatabase` to `true` in appsettings.json.

### 3. Configuration

Update connection strings and settings in `appsettings.json` files for each API:

- `ApiLayer/REST/appsettings.json`
- `ApiLayer/SOAP/appsettings.json`
- `ApiLayer/gRPC/appsettings.json`

Key configuration sections:
- `ConnectionStrings:DefaultConnection` - Database connection
- `Authentication:Jwt:SecretKey` - JWT secret (change in production!)
- `ExchangeRateProviders` - Provider configurations
- `SystemConfiguration` - System behavior settings

### 4. Build the Solution

```bash
dotnet restore
dotnet build
```

### 5. Run the APIs

#### REST API
```bash
cd ApiLayer/REST
dotnet run
```
Access at:
- API: `http://localhost:5000`
- Swagger UI: `http://localhost:5000/scalar/v1`
- Hangfire Dashboard: `http://localhost:5000/hangfire`

#### SOAP API
```bash
cd ApiLayer/SOAP
dotnet run
```
Access at:
- WSDL: `http://localhost:5001/?wsdl`

#### gRPC API
```bash
cd ApiLayer/gRPC
dotnet run
```
Access at:
- gRPC endpoint: `http://localhost:5002`

### 6. Default Credentials

The system seeds default users on startup:

**Admin Account:**
- Email: `admin@example.com`
- Password: `Admin@123`
- Role: Admin

**Consumer Account:**
- Email: `user@example.com`
- Password: `User@123`
- Role: Consumer

## Interactive Test Console

The solution includes a powerful interactive console application for testing all APIs:

```bash
cd TestLayer/ConsoleTestApp
dotnet run
```

### Console Commands

```
Authentication:
  login-all <email>              - Login to all APIs
  logout-all                     - Logout from all APIs

Testing:
  test-all                       - Test all endpoints across protocols
  test-rest                      - Test REST endpoints
  test-soap                      - Test SOAP endpoints
  test-grpc                      - Test gRPC endpoints

Exchange Rates:
  rates                          - Get latest rates from all APIs
  rates-rest                     - Get latest rates from REST
  rates-soap                     - Get latest rates from SOAP
  rates-grpc                     - Get latest rates from gRPC
  historical <from> <to>         - Get historical rates
  convert <amount> <from> <to>   - Convert currency

Data Management:
  currencies                     - List all currencies
  providers                      - List all providers
  users                          - List all users
  health                         - Get system health

Utilities:
  compare                        - Compare API performance
  help                           - Show all commands
  exit                           - Exit application
```

### Example Session

```bash
> login-all admin@example.com
✓ Successfully logged in to all protocols

> test-all
Testing REST endpoints...
Testing SOAP endpoints...
Testing GRPC endpoints...

Summary:
✓ REST: 12/12 passed
✓ SOAP: 12/12 passed
✓ GRPC: 12/12 passed

> convert 100 EUR USD
Converting 100 EUR to USD...
REST:  108.50 USD (rate: 1.0850)
SOAP:  108.50 USD (rate: 1.0850)
gRPC:  108.50 USD (rate: 1.0850)
```

## API Documentation

### REST API

Once running, access the interactive API documentation:
- **Scalar UI**: `http://localhost:5000/scalar/v1` (recommended)
- **OpenAPI JSON**: `http://localhost:5000/openapi/v1.json`

Key endpoints:
- `POST /api/auth/login` - Authenticate
- `GET /api/exchange-rates/latest/all/grouped` - Latest rates
- `GET /api/exchange-rates/current/grouped` - Current rates
- `GET /api/exchange-rates/history/grouped` - Historical rates
- `GET /api/exchange-rates/convert` - Currency conversion
- `GET /api/currencies` - List currencies
- `GET /api/providers` - List providers
- `GET /api/system-health` - System health

### SOAP API

Access WSDL at `http://localhost:5001/?wsdl`

Services available:
- AuthenticationService
- ExchangeRateService
- CurrencyService
- ProviderService
- UserService
- SystemHealthService

### gRPC API

Proto definitions in `ApiLayer/gRPC/Protos/`:
- `authentication.proto`
- `exchange_rates.proto`
- `currencies.proto`
- `providers.proto`
- `users.proto`
- `system_health.proto`

## Background Jobs

The system uses Hangfire for background job processing:

### Scheduled Jobs

- **Exchange Rate Fetching**: Runs daily at configured times per provider
  - ECB: 16:00 CET
  - CNB: 14:30 CET
  - BNR: 13:00 EET

- **Health Checks**: Every 5 minutes
- **Data Cleanup**: Daily cleanup of old data based on retention policies

### Hangfire Dashboard

Monitor jobs at `http://localhost:5000/hangfire`
- View job history
- Manually trigger jobs
- Monitor failures and retries

## Configuration Reference

### System Configuration

```json
{
  "SystemConfiguration": {
    "ProviderHealth": {
      "AutoDisableAfterFailures": 10,
      "HealthCheckIntervalMinutes": 15,
      "StaleDataThresholdHours": 48
    },
    "DataRetention": {
      "RetainExchangeRatesDays": 730,
      "RetainFetchLogsDays": 90,
      "RetainErrorLogsDays": 30,
      "EnableAutoCleanup": true
    },
    "BackgroundJobs": {
      "FetchHistoricalOnStartup": true,
      "HistoricalDataDays": 90,
      "DefaultRetryDelayMinutes": 30,
      "MaxRetries": 3
    }
  }
}
```

### Exchange Rate Providers

Each provider can be configured independently:

```json
{
  "ExchangeRateProviders": {
    "ECB": {
      "IsActive": true,
      "Url": "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
      "Configuration": {
        "UpdateTime": "16:00",
        "TimeZone": "CET"
      }
    }
  }
}
```

## Testing

### Unit Tests

```bash
cd TestLayer/Unit
dotnet test
```

### Integration Tests

```bash
cd TestLayer/Integration
dotnet test
```

### Manual Testing

Use the ConsoleTestApp for comprehensive manual testing across all protocols.

## Troubleshooting

### Database Connection Issues

```bash
# Restart LocalDB
sqllocaldb stop MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
```

### Port Conflicts

Default ports:
- REST: 5000
- SOAP: 5001
- gRPC: 5002

Change ports in `launchSettings.json` or `appsettings.json` if needed.

### Background Jobs Not Running

1. Check Hangfire dashboard at `/hangfire`
2. Verify database connection
3. Check provider URLs are accessible
4. Review logs for errors

## Development

### Adding a New Provider

1. Create provider class in `ExchangeRateProviderLayer/`
2. Implement `IExchangeRateProvider` interface
3. Add configuration to `appsettings.json`
4. Register in DI container in `Program.cs`
5. Add background job configuration

### Project Structure

- **CQRS Pattern**: Commands and Queries separated
- **MediatR**: Request/response pipeline
- **FluentValidation**: Input validation
- **EF Core**: Data access with repository pattern
- **Hangfire**: Background job processing
- **SignalR**: Real-time updates (REST API)

## Performance

The system is designed for high performance:
- Efficient database queries with proper indexing
- Caching of configuration data
- Batch operations for historical data
- Background processing for heavy operations
- Connection pooling

## Security Considerations

- Change JWT secret key in production
- Use HTTPS in production
- Configure rate limiting appropriately
- Review and adjust password policies
- Enable authentication on Hangfire dashboard in production
- Regularly update retention policies
- Monitor provider health and errors

## License

[Specify your license here]

## Contributing

[Specify contribution guidelines here]

## Support

For issues, questions, or contributions, please [specify contact method or issue tracker].
