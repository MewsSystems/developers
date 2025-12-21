# Exchange Rate Provider - Czech National Bank (CNB)

## Introduction

This project implements a production-ready Exchange Rate Provider for the Czech National Bank (CNB), designed as a programming exercise with real-world applications. The solution provides exchange rate data by consuming the official CNB API and delivers it through both a REST API and a console application.

### Project Overview

The Exchange Rate Provider is built following **Clean Architecture** principles, ensuring separation of concerns, testability, and maintainability. The solution is organized into the following projects:

#### Source Projects (`src/`)

- **ExchangeRateProvider.Domain**: Core domain entities and business rules (Currency, ExchangeRate)
- **ExchangeRateProvider.Application**: Application services, interfaces, and use cases
- **ExchangeRateProvider.Infrastructure**: External integrations including CNB API client, caching, and resilience policies
- **ExchangeRateProvider.Api**: REST API with endpoints, authentication, rate limiting, and middleware
- **ExchangeRateProvider.ConsoleApp**: Console application to demonstrate functionality via command line interface

#### Test Projects (`test/`)

- **ExchangeRateProvider.Domain.Tests**: Unit tests for domain entities
- **ExchangeRateProvider.Application.UnitTests**: Unit tests for application services
- **ExchangeRateProvider.Infrastructure.Tests**: Tests for infrastructure components and external integrations
- **ExchangeRateProvider.Api.Tests**: Integration tests for API endpoints

The application provides exchange rate data through the following interfaces:

- **REST API**: Exposes an exchange rate endpoint for external consumers
- **Console Application**: Command-line interface for direct exchange rate queries

### Data Source

The application consumes exchange rate data from the Czech National Bank's official API:

- **API Documentation**: https://api.cnb.cz/cnbapi/swagger-ui.html#/%2Fexrates/dailyUsingGET_1
- **Data Provider**: Czech National Bank
- **Update Frequency**: Exchange rates are updated once daily on weekdays after 2:30 PM CET. No updates occur on weekends or public holidays.

The implementation is built with production-grade considerations including error handling, caching, rate limiting, authentication, and comprehensive testing coverage.

## Architecture & Design Decisions

### Minimal APIs Approach

We chose to implement **ASP.NET Core Minimal APIs** instead of traditional controllers. This decision provides several benefits:

- **Modern .NET Approach**: Explores the latest API development patterns introduced in .NET 6+
- **Reduced Boilerplate**: Less ceremony and cleaner endpoint definitions
- **Performance**: Slightly better performance due to reduced overhead
- **Simplicity**: More direct mapping between HTTP endpoints and business logic

```csharp
// Example from ExchangeRateEndpoints.cs
app.MapGet("/exchange-rates", async (
    [FromQuery] string currencies,
    IExchangeRateProviderService service,
    CancellationToken cancellationToken) => 
{
    // Direct endpoint implementation
});
```

### Interactive API Documentation with Scalar

As this project serves as a learning exercise, for API documentation, we implemented **Scalar** instead of traditional Swagger UI or NSwag. This choice was made to:

- **Explore Modern Tooling**: Scalar provides a more modern, interactive documentation experience
- **Enhanced Developer Experience**: Better UI/UX compared to traditional Swagger implementations
- **OpenAPI Integration**: Seamless integration with .NET's built-in OpenAPI support

```csharp
// API documentation setup in Program.cs
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();              // Built-in .NET OpenAPI support
    app.MapScalarApiReference();   // Scalar interactive documentation
}
```

This combination provides developers with an excellent interactive experience for exploring and testing the API endpoints directly from the browser.

#### API Documentation Access Points

When running the API in development mode, you can access:

- **OpenAPI Definition**: `/openapi/v1.json` - Raw OpenAPI specification in JSON format
- **Scalar Interactive Documentation**: `/scalar/` - Modern, interactive API documentation interface
- **HTTP File Testing**: `src/ExchangeRateProvider.Api/ExchangeRateProvider.http` - Pre-configured HTTP requests for testing, making it easy to test the API functionality directly from your IDE.

### Intelligent Caching Strategy

One of the key architectural decisions was implementing an intelligent **in-memory caching** layer to avoid excessive calls to the CNB API. The CNB website explicitly states:

> *"Unjustified and excessive access to this site will be considered undesirable and will be restricted as part of the security measures applied."*

To respect this requirement and optimize performance, we implemented an in-memory caching mechanism with the following components:

#### Cache Implementation Components

**1. CnbCacheDateTimeProvider**
- Calculates optimal cache expiration times based on CNB's fixing schedule
- Handles Prague timezone conversions and daylight saving time automatically
- Accounts for business days vs. weekends/holidays
- Provides configurable buffer time (default: 5 minutes) after the 2:30 PM fixing time

**2. CnbCachedExchangeRateProviderDecorator**
- Implements the Decorator pattern to add caching
- Uses multiple cache keys for efficient data retrieval and management
- Prevents concurrent API calls using `GetOrCreateAsync` pattern
- Automatically invalidates cache based on CNB's schedule

#### Cache Expiration Logic

The cache intelligently calculates when to expire based on CNB's publishing schedule:

- **Business Days (Mon-Fri)**: 
  - Before 2:30 PM Prague time → Cache expires at 2:35 PM (2:30 PM + 5min buffer)
  - After 2:30 PM Prague time → Cache expires next business day at 2:35 PM
- **Weekends**: Cache expires on next Monday at 2:35 PM Prague time
- **Public Holidays**: Simplified approach - treats as regular weekends (next business day expiration)

#### Holiday Handling Decision

We deliberately chose **not** to integrate with external holiday services for Czech public holidays. This decision was made because:

1. **Minimal Impact**: A single API call on a holiday has negligible performance impact
2. **Simplicity**: Avoids external dependencies and potential service failures  
3. **Reliability**: Reduces points of failure in the system
4. **Cost-Benefit**: The complexity of holiday service integration outweighs the marginal benefit

The system treats holidays as regular weekend days, ensuring cache expiration on the next business day when CNB resumes publishing rates.

#### Cache Key Strategy

```
ExchangeRates:LatestValidFor → "2024-01-15"
ExchangeRates:ValidFor:2024-01-15 → [Exchange Rate List]
ExchangeRates:LatestValid:Loader → Loader lock for concurrency
```

This multi-key approach enables:
- Fast lookups for the latest valid date
- Efficient storage of historical exchange rates  
- Prevention of concurrent API calls through loader locking
- Automatic cleanup through time-based expiration

### Design Patterns Implementation

The solution leverages several well-established design patterns to ensure maintainability, extensibility, and separation of concerns:

#### Decorator Pattern

The **Decorator Pattern** is implemented through `CnbCachedExchangeRateProviderDecorator` to add caching functionality without modifying the original exchange rate provider:

```csharp
// Decorator wraps any IExchangeRateProvider implementation
internal class CnbCachedExchangeRateProviderDecorator(
    IExchangeRateProvider inner,
    IMemoryCache cache,
    ICacheDateTimeProvider dateTimeProvider,
    ILogger<CnbCachedExchangeRateProviderDecorator> logger)
    : IExchangeRateProvider
{
    public async Task<IReadOnlyList<ExchangeRate>> GetLatestAsync(CancellationToken cancellationToken = default)
    {
        // Cache logic wraps the inner provider call
        return await cache.GetOrCreateAsync(LoaderKey, async entry =>
        {
            return await inner.GetLatestAsync(cancellationToken);
        });
    }
}
```

**Benefits:**
- **Single Responsibility**: Caching logic is separate from data fetching logic
- **Open/Closed Principle**: Can add caching to any provider without modification
- **Composability**: Can stack multiple decorators (e.g., retry + cache + logging)

#### Strategy Pattern

The **Strategy Pattern** is implemented through the `IExchangeRateProvider` interface with `CnbExchangeRateProvider` as a concrete strategy:

```csharp
// Strategy interface
public interface IExchangeRateProvider
{
    Task<IReadOnlyList<ExchangeRate>> GetLatestAsync(CancellationToken cancellationToken = default);
}

// Concrete strategy for CNB
internal class CnbExchangeRateProvider : IExchangeRateProvider
{
    // CNB-specific implementation
}
```

**Benefits:**
- **Flexibility**: Easy to switch between different exchange rate providers
- **Extensibility**: New providers (ECB, Federal Reserve, etc.) can be added without changing existing code
- **Testability**: Strategies can be easily mocked and tested independently

#### Options Pattern

The **Options Pattern** is extensively used throughout the application for strongly-typed configuration management, following .NET best practices:

```csharp
// Configuration class with validation attributes
public record CnbApiOptions
{
    public const string SectionName = "CnbApi";
    
    public required string HttpClientName { get; init; }
    public required string BaseUrl { get; init; }
    public required string DailyExchangeRatesEndpoint { get; init; }
}

// Registration in ServiceCollectionExtensions
services.Configure<CnbApiOptions>(configuration.GetSection(CnbApiOptions.SectionName));

// Usage with dependency injection
public class CnbExchangeRateProvider(IOptions<CnbApiOptions> options, HttpClient httpClient)
{
    private readonly CnbApiOptions _options = options.Value;
    
    public async Task<IReadOnlyList<ExchangeRate>> GetLatestAsync(CancellationToken cancellationToken = default)
    {
        var endpoint = _options.BaseUrl + _options.DailyExchangeRatesEndpoint;
        // Use configured values...
    }
}
```

**Configuration Structure:**
```json
{
  "Auth0": {
    "Authority": "https://dev-ysubdf4h0li5mmwc.us.auth0.com/",
    "Audience": "exchange-rate-api"
  },
  "CnbApi": {
    "HttpClientName": "CnbApiHttpClient",
    "BaseUrl": "https://api.cnb.cz",
    "DailyExchangeRatesEndpoint": "/cnbapi/exrates/daily"
  }
}
```

**Benefits:**
- **Type Safety**: Eliminates magic strings and provides compile-time validation
- **Validation**: Built-in support for configuration validation using data annotations
- **Testability**: Easy to mock and test with different configuration values
- **Hot Reload**: Supports configuration changes without application restart (with `IOptionsMonitor`)
- **Environment-Specific**: Different configurations per environment (Development, Production)

#### Future Factory Pattern Consideration

While not currently implemented, the **Factory Pattern** is planned for future releases to manage multiple exchange rate provider strategies:

```csharp
// Future implementation concept
public interface IExchangeRateProviderFactory
{
    IExchangeRateProvider CreateProvider(ExchangeRateProviderType type);
}

public enum ExchangeRateProviderType
{
    CzechNationalBank,
    EuropeanCentralBank,
    FederalReserve,
    // Additional providers...
}
```

**Future Benefits:**
- **Centralized Creation**: Single point for provider instantiation and configuration
- **Dynamic Selection**: Runtime selection of providers based on configuration or requirements
- **Dependency Management**: Proper injection of provider-specific dependencies

This pattern will become valuable when supporting multiple central banks or financial data providers, allowing the application to seamlessly switch between different data sources based on configuration or business requirements.

## Technology Stack & Deployment

### Technology Stack

- **.NET 10**: Latest Long Term Support (LTS) version providing enhanced performance, security, and long-term support
- **ASP.NET Core**: For REST API implementation with built-in dependency injection, middleware pipeline, and health checks
- **Docker**: Containerization for consistent deployment across environments
- **Auth0**: Authentication and authorization for API security
- **Memory Caching**: In-memory caching for improved performance and reduced API calls
- **Resilience Patterns**: Circuit breaker, retry policies with exponential backoff, and timeout handling for external API integration

### Authentication

The API is secured using **Auth0** with JWT tokens and client credentials flow. An Auth0 application has been configured to generate JWT tokens for API access.

#### Obtaining an Access Token

To call the API endpoints, you need to obtain a valid JWT token from Auth0:

```bash
curl --request POST \
  --url https://dev-ysubdf4h0li5mmwc.us.auth0.com/oauth/token \
  --header 'content-type: application/json' \
  --data '{
    "client_id":"JjZobrdF99vYG7iV34UPNB5OxKbhzSPA",
    "client_secret":"NsfcnUI-IWpufHp9CvCRXDOW5sN924lC7X-XhZaudhh99mGwyf1GnIVSRKyvk8pQ",
    "audience":"exchange-rate-api",
    "grant_type":"client_credentials"
  }'
```

This will return a JSON response containing the access token:

```json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjZRTG94QUQ0MUM5bGlMVnBlbHpSbSJ9.eyJpc3MiOiJodHRwczovL2Rldi15c3ViZGY0aDBsaTVtbXdjLnVzLmF1dGgwLmNvbS8iLCJzdWIiOiJKalpvYnJkRjk5dllHN2lWMzRVUE5CNU94S2JoelNQQUBjbGllbnRzIiwiYXVkIjoiZXhjaGFuZ2UtcmF0ZS1hcGkiLCJpYXQiOjE3NjYzMTc2NzcsImV4cCI6MTc2NjQwNDA3NywiZ3R5IjoiY2xpZW50LWNyZWRlbnRpYWxzIiwiYXpwIjoiSmpab2JyZEY5OXZZRzdpVjM0VVBOQjVPeEtiaHpTUEEifQ.eblxRmlkuoZ_2LaS5gueKy6XBSYB5fX6LWiIevRG_CtlHIKkn-4aFQKbIxV2IH4p3uKnfB2uercybjb85RkxBy7HMLeP0kfEl-XDbqf4T1eNOhOe4kyck0a6oRKyZsBDuIWZJbYLBiD7s505Ysz1Gaik2ZPfBAr0ZtejlM4DRMrbWRLWYFJ2ElcHjG4dCkBbwcI8nLO0iKwAIyTqUch9YcAQrUJsEPs0d9EQROlpXpDsaXnENHAXt-wtXV-AXju55WZlkmes3selWICeKKPV-AZBYvV8a8dEPU5gHjIOx3e8jptiyA2QI4dJLRnHgU4FL-HgbBzIh97SSD3k7KUPWA",
  "token_type": "Bearer"
}
```

Use the `access_token` value in your API requests as shown in the examples below.

### Docker Deployment

Both applications are containerized using Docker for consistent deployment across different environments.

#### Console Application

Build and run the console application:

```bash
# Build the Docker image
docker build -f src/ExchangeRateProvider.ConsoleApp/Dockerfile -t exchange-rate-provider-console-app:latest .

# Run the container
docker run --rm exchange-rate-provider-console-app:latest
```

#### API Application

Build and run the API application:

```bash
# Build the Docker image
docker build -f src/ExchangeRateProvider.Api/Dockerfile -t exchange-rate-provider-api:latest .

# Run the container with port mapping
docker run --rm -p 8080:8080 -p 8081:8081 -e ASPNETCORE_ENVIRONMENT=Development exchange-rate-provider-api:latest
```

#### Testing the API

Once the API is running, you can test it using curl with a valid access token:

```bash
curl -X GET "http://localhost:8080/exchange-rates?currencies=USD,EUR,GBP,XYZ" \
     -H "Accept: application/json" \
     -H "Authorization: Bearer your_actual_access_token_here"
```

The API also includes a health check endpoint (no authentication required):

```bash
curl -X GET "http://localhost:8080/health"