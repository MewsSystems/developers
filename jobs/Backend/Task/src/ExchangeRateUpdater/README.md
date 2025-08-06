# Exchange Rate Updater

A .NET 8 console application that fetches and manages exchange rates from multiple providers using Clean Architecture principles and Domain-Driven Design (DDD) concepts.

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022, VS Code, or any .NET-compatible IDE

### Running Locally

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Task
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run --project src/ExchangeRateUpdater
   ```

4. **Run tests**
   ```bash
   dotnet test
   ```

## 🏗️ Architecture Overview

This project follows **Clean Architecture** principles with **Domain-Driven Design (DDD)** concepts, organized into distinct layers:

### Layer Structure

```
src/
├── ExchangeRateUpdater/                    # Application Layer (Entry Point)
├── ExchangeRateUpdater.Domain/            # Domain Layer (Core Business Logic)
├── ExchangeRateUpdater.Infrastructure/    # Infrastructure Layer (External Concerns)
└── ExchangeRateUpdater.Infrastructure.Providers/  # Provider Implementations
```

### Layer Responsibilities

#### 1. **Domain Layer** (`ExchangeRateUpdater.Domain`)
- **Purpose**: Core business logic and domain models
- **DDD Concepts Implemented**:
  - **Value Objects**: `Currency`, `ExchangeRate` with validation
  - **Domain Services**: `ICacheService` interface
  - **Repositories**: `IExchangeRateRepository`, `IExchangeRateProvider` interfaces
  - **Invariants**: Currency codes must be 3 characters, exchange values must be positive

#### 2. **Infrastructure Layer** (`ExchangeRateUpdater.Infrastructure`)
- **Purpose**: External concerns, data persistence, caching
- **Components**:
  - Repository implementations
  - Caching services (`DistributedCacheService`)
  - Cache key generation utilities

#### 3. **Infrastructure.Providers Layer** (`ExchangeRateUpdater.Infrastructure.Providers`)
- **Purpose**: External API integrations with anti-corruption layer
- **Components**:
  - Provider-specific implementations
  - API client interfaces
  - Response model mappings

#### 4. **Application Layer** (`ExchangeRateUpdater`)
- **Purpose**: Application entry point and orchestration
- **Components**:
  - `Program.cs` with `IHostBuilder`
  - `ExchangeRateProvider` (application service)
  - Dependency injection configuration
  - **OpenTelemetry Middleware** for observability

## 🔧 Technology Stack

### Core Technologies
- **.NET 8.0** - Latest LTS version
- **C# 12** - Modern C# features and syntax

### HTTP Client & API Integration
- **Refit** - Type-safe REST client library
  - Custom implementation for clean, maintainable code
  - Avoided NSwag-generated code for better control and readability
- **HttpClientFactory** - Managed HTTP client lifecycle
- **Polly** - Resilience and transient fault handling

### Caching
- **IDistributedCache** - Distributed caching abstraction
- **MemoryDistributedCache** - In-memory implementation for development
- **System.Text.Json** - JSON serialization for cache storage

### Dependency Injection & Configuration
- **Microsoft.Extensions.DependencyInjection** - Built-in DI container
- **Microsoft.Extensions.Hosting** - Host builder pattern
- **Microsoft.Extensions.Configuration** - Configuration management
- **Microsoft.Extensions.Options** - Strongly-typed configuration

### Logging & Observability
- **Microsoft.Extensions.Logging** - Structured logging
- **OpenTelemetry** - Observability framework with correlation IDs
- **Built-in .NET logging** - Console and debug output

### Testing
- **xUnit** - Unit testing framework
- **Moq** - Mocking library
- **Microsoft.NET.Test.Sdk** - Test discovery and execution

## 🔍 OpenTelemetry Configuration

### Overview
OpenTelemetry is **enabled by default** in this application to provide **correlated logs** and **distributed tracing**. This ensures that all operations across different services and components can be traced and correlated using unique trace IDs.

### Why OpenTelemetry?
- **Correlation IDs**: Every log entry and trace span has a unique correlation ID (if deployed in web)
- **Distributed Tracing**: Track requests across multiple services and components (if deployed in web)
- **Observability**: Complete visibility into application behavior
### Middleware Implementation
The OpenTelemetry configuration is centralized in the `OpenTelemetryMiddleware` class:

```csharp
// In Program.cs
logging.AddOpenTelemetryLogging(hostContext.Configuration);
services.AddOpenTelemetryServices(hostContext.Configuration);
```

### Default Configuration
By default, the application is configured with these settings:
- **Log Level**: Warning (reduces noise)
- **Tracing**: Disabled (performance optimization)
- **Logging**: Enabled with correlation IDs
- **Console Exporters**: Disabled for tracing, enabled for logging

### Configuration Structure
```json
{
  "OpenTelemetry": {
    "Enabled": true,
    "ServiceName": "ExchangeRateUpdater",
    "ServiceVersion": "1.0.0",
    "ResourceAttributes": {
      "deployment.environment": "development"
    },
    "Tracing": {
      "Enabled": false,
      "ConsoleExporter": {
        "Enabled": false
      },
      "HttpClientInstrumentation": {
        "Enabled": false
      }
    },
    "Logging": {
      "Enabled": true,
      "ConsoleExporter": {
        "Enabled": true
      }
    }
  }
}
```

### Configuration Options

#### **Global Settings**
- `Enabled`: Master switch for OpenTelemetry (default: `true`)
- `ServiceName`: Application service name
- `ServiceVersion`: Application version
- `ResourceAttributes`: Additional metadata for traces and logs

#### **Tracing Configuration**
- `Tracing.Enabled`: Enable/disable distributed tracing (default: `false`)
- `Tracing.ConsoleExporter.Enabled`: Output traces to console (default: `false`)
- `Tracing.HttpClientInstrumentation.Enabled`: Auto-instrument HTTP calls (default: `false`)

#### **Logging Configuration**
- `Logging.Enabled`: Enable/disable OpenTelemetry logging (default: `true`)
- `Logging.ConsoleExporter.Enabled`: Output logs to console (default: `true`)

### Development Configuration
For development and debugging, you can enable tracing and lower log levels:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "ExchangeRateUpdater": "Information"
    }
  },
  "OpenTelemetry": {
    "Tracing": {
      "Enabled": true,
      "ConsoleExporter": {
        "Enabled": true
      },
      "HttpClientInstrumentation": {
        "Enabled": true
      }
    }
  }
}
```

### Sample Output
When OpenTelemetry is enabled, you'll see structured logs like:
```
LogRecord.Timestamp: 2025-08-06T18:30:15.4913160Z
LogRecord.CategoryName: ExchangeRateUpdater.Program
LogRecord.Severity: Information
LogRecord.Body: Starting exchange rate retrieval process
Resource associated with LogRecord:
  service.name: ExchangeRateUpdater
  service.version: 1.0.0
  service.instance.id: 9e81e998-a3d1-485f-8bd8-a1f40f03075b
  telemetry.sdk.name: opentelemetry
  telemetry.sdk.language: dotnet
  telemetry.sdk.version: 1.9.0
```

### Disabling OpenTelemetry
To disable OpenTelemetry completely, set the configuration:
```json
{
  "OpenTelemetry": {
    "Enabled": false
  }
}
```

## 🛡️ Anti-Corruption Layer Design (Attempt :D)

### CNB Provider Implementation

The Czech National Bank (CNB) provider demonstrates the anti-corruption layer pattern:

#### 1. **External API Interface** (`ICzechNationalBankApiClient`)
```csharp
public interface ICzechNationalBankApiClient
{
    Task<CnbExchangeRateResponse> GetFrequentExchangeRatesAsync(string date);
    Task<CnbExchangeRateResponse> GetOtherExchangeRatesAsync(string month);
}
```

#### 2. **External Response Models**
```csharp
public class CnbExchangeRateResponse
{
    public CnbExchangeRateModel[] Rates { get; set; }
}

public class CnbExchangeRateModel
{
    public string CurrencyCode { get; set; }
    public decimal Rate { get; set; }
    public int Amount { get; set; }
    // ... other external properties
}
```

#### 3. **Domain Model Conversion**
```csharp
public class CnbExchangeRateModel
{
    public ExchangeRate ToExchangeRate()
    {
        return new ExchangeRate(
            new Currency(CurrencyCode), 
            new Currency("CZK"), 
            Rate / Amount, 
            "CNB", 
            DateTime.Parse(ValidFor)
        );
    }
}
```

#### 4. **Provider Implementation**
```csharp
public class CnbExchangeRateProvider : IExchangeRateProvider
{
    private readonly ICzechNationalBankApiClient _apiClient;
    
    public async Task<ExchangeRate[]> FetchAllCurrentAsync()
    {
        var response = await _apiClient.GetFrequentExchangeRatesAsync(date);
        return response.Rates.Select(r => r.ToExchangeRate()).ToArray();
    }
}
```

## 📋 Adding New Providers

### Step-by-Step Guide

#### 1. **Create Provider Interface** (if needed)
```csharp
// In ExchangeRateUpdater.Infrastructure.Providers/ExchangeRates/NewProvider/
public interface INewProviderApiClient
{
    Task<NewProviderResponse> GetExchangeRatesAsync(string date);
}
```

#### 2. **Create Response Models**
```csharp
public class NewProviderResponse
{
    public NewProviderRate[] Rates { get; set; }
}

public class NewProviderRate
{
    public string FromCurrency { get; set; }
    public string ToCurrency { get; set; }
    public decimal Rate { get; set; }
    
    public ExchangeRate ToExchangeRate()
    {
        return new ExchangeRate(
            new Currency(FromCurrency),
            new Currency(ToCurrency),
            Rate,
            "NewProvider"
        );
    }
}
```

#### 3. **Implement Provider**
```csharp
public class NewProviderExchangeRateProvider : IExchangeRateProvider
{
    public string Name => "NewProvider";
    public string DefaultLanguage => "EN";
    public string DefaultCurrency => "USD";
    
    private readonly INewProviderApiClient _apiClient;
    private readonly ICacheService _cacheService;
    
    public NewProviderExchangeRateProvider(
        INewProviderApiClient apiClient,
        ICacheService cacheService)
    {
        _apiClient = apiClient;
        _cacheService = cacheService;
    }
    
    public async Task<ExchangeRate[]> FetchAllCurrentAsync()
    {
        var response = await _apiClient.GetExchangeRatesAsync(DateTime.UtcNow.ToString("yyyy-MM-dd"));
        return response.Rates.Select(r => r.ToExchangeRate()).ToArray();
    }
}
```

#### 4. **Register in DI Container**
```csharp
// In ExchangeRateUpdater.Infrastructure.Providers/Middleware/DependencyInjection.cs
public static IServiceCollection AddInfrastructureProviders(this IServiceCollection services, IConfiguration configuration)
{
    // Register API client
    services.AddRefitClient<INewProviderApiClient>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.newprovider.com"))
        .AddPolicyHandler(GetRetryPolicy());
    
    // Register provider
    services.AddScoped<IExchangeRateProvider, NewProviderExchangeRateProvider>();
    
    return services;
}
```

#### 5. **Add Configuration** (if needed)
```json
// In appsettings.json
{
  "ExchangeRateProviders": {
    "NewProvider": {
      "BaseUrl": "https://api.newprovider.com",
      "TimeoutSeconds": 30,
      "Cache": {
        "AbsoluteExpirationInMinutes": 60,
        "SlidingExpirationInMinutes": 15
      }
    }
  }
}
```

### Provider Requirements

1. **Implement `IExchangeRateProvider` interface**
2. **Use anti-corruption layer pattern**
3. **Implement caching strategy**
4. **Handle errors gracefully**
5. **Add comprehensive logging**
6. **Write unit tests**

## 🔄 File Structure Changes

### Before (Monolithic)
```
src/
└── ExchangeRateUpdater/
    ├── Program.cs
    ├── ExchangeRateProvider.cs
    └── Models/
```

### After (_attempted_ Clean Architecture)
```
src/
├── ExchangeRateUpdater/                    # Application Layer
│   ├── Program.cs
│   ├── ExchangeRateProvider.cs
│   ├── appsettings.json
│   └── Middleware/
│       ├── DependencyInjection.cs
│       └── OpenTelemetryMiddleware.cs      # Centralized OpenTelemetry config
├── ExchangeRateUpdater.Domain/            # Domain Layer
│   ├── Models/
│   │   ├── Currency.cs
│   │   └── ExchangeRate.cs
│   ├── Providers/
│   │   └── IExchangeRateProvider.cs
│   ├── Repositories/
│   │   └── IExchangeRateRepository.cs
│   └── Services/
│       └── ICacheService.cs
├── ExchangeRateUpdater.Infrastructure/    # Infrastructure Layer
│   ├── Repositories/
│   │   └── ExchangeRateRepository.cs
│   ├── Services/
│   │   ├── DistributedCacheService.cs
│   │   └── CacheKeyGenerator.cs
│   └── Middleware/
│       └── DependencyInjection.cs
└── ExchangeRateUpdater.Infrastructure.Providers/  # Provider Layer
    ├── ExchangeRates/
    │   └── CzechNationalBank/
    │       ├── CnbExchangeRateProvider.cs
    │       ├── ICzechNationalBankApiClient.cs
    │       └── Models/
    │           ├── ExchangeRateResponse.cs
    │           └── CzechNationalBankExchangeRateConfig.cs
    ├── RefitLoggingHandler.cs
    ├── ExchangeRateProvidersConfig.cs
    └── Middleware/
        └── DependencyInjection.cs
```

## 🧪 Testing Strategy

### Test Projects
```
tests/
├── ExchangeRateUpdater.Domain.Tests/           # Domain layer tests
├── ExchangeRateUpdater.Infrastructure.Tests/   # Infrastructure layer tests
├── ExchangeRateUpdater.Infrastructure.Providers.Tests/  # Provider tests
└── ExchangeRateUpdater.Tests/                  # Application layer tests
```

### Testing Approach
- **Unit Tests**: Each layer tested independently
- **Mock Testing**: External dependencies mocked
- **Real Cache Testing**: Using `MemoryDistributedCache` for realistic testing
  - _PS: I had troubles mocking and I ran out of time_

## 🤔 Assumptions and Decisions

### CNB Provider Implementation Decisions

#### 1. **Future Date Handling**
- **Decision**: Default to current date when future dates are provided
- **Reasoning**: The CNB API doesn't support future dates and returns current rates instead
- **Implementation**: 
  ```csharp
  public async Task<ExchangeRate[]> FetchByDateAsync(DateTime date)
  {
      var requestDate = date > DateTime.UtcNow ? DateTime.UtcNow : date;
      // ... API call with requestDate
  }
  ```

#### 2. **Monthly Exchange Rates (Uncommon Currencies)**
- **Decision**: Return empty results when monthly data is unavailable
- **Reasoning**: 
  - Monthly rates for uncommon currencies are not always available especially during the first few days of a month
  - Availability depends on the time of month
  - Returning previous month's data would be inaccurate
  - Better to return empty than provide stale/inaccurate data

#### 3. **Exchange Rate Status Tracking**
- **Decision**: Not implemented in current version
- **Reasoning**: 
  - Need to validate this corner case with product
  - Would add complexity to domain models
  - Requires additional validation logic
  - Could be implemented as future enhancement
- **Potential Implementation**:
  ```csharp
  public enum ExchangeRateStatus
  {
      Active,
      Outdated,
      Expired
  }
  
  public record ExchangeRate
  {
      public ExchangeRateStatus Status { get; }
      public DateTime LastUpdated { get; }
      // ... other properties
  }
  ```

### General Architecture Decisions

#### 1. **Caching Strategy**
- **Decision**: Use IDistributedCache with configurable expiration
- **Reasoning**: 
  - Supports both in-memory and distributed scenarios
  - Configurable expiration times for different data types
  - Graceful degradation when cache is unavailable

#### 2. **Error Handling**
- **Decision**: Log warnings and return empty results for non-critical failures
- **Reasoning**: 
  - Prevents application crashes from external API issues
  - Maintains service availability
  - Provides visibility through logging

#### 3. **Provider Selection**
- **Decision**: Chain of Responsibility pattern for provider management
- **Reasoning**: 
  - Allows dynamic provider selection
  - Supports multiple providers simultaneously
  - Easy to extend with new providers

#### 4. **OpenTelemetry Integration**
- **Decision**: Enabled by default with centralized middleware, tracing disabled by default
- **Reasoning**:
  - Provides correlated logs for better debugging
  - Enables distributed tracing across services when needed
  - Enterprise-grade observability standards
  - Centralized configuration management
  - Performance optimization by default (tracing disabled)
  - Can be easily enabled for development/debugging

#### 5. **Default Logging Configuration**
- **Decision**: Warning level by default for production-like behavior
- **Reasoning**:
  - Reduces noise in production environments
  - Focuses on important events and errors
  - Can be easily adjusted for development
  - Balances observability with performance

## 🚧 Future Improvements

### Potential Enhancements

1. **Builder Pattern for Domain Models**
   ```csharp
   public class ExchangeRateBuilder
   {
       private Currency _sourceCurrency;
       private Currency _targetCurrency;
       private decimal _exchangeValue;
       
       public ExchangeRateBuilder WithSourceCurrency(string code)
       {
           _sourceCurrency = new Currency(code);
           return this;
       }
       
       public ExchangeRate Build()
       {
           return new ExchangeRate(_sourceCurrency, _targetCurrency, _exchangeValue);
       }
   }
   ```

2. **Enhanced Mocking for IDistributedCache**
   - Create custom mock implementations
   - Test cache expiration scenarios
   - Verify cache key generation

3. **Additional Provider Implementations**
   - European Central Bank (ECB)
   - Federal Reserve (Fed)
   - Bank of England (BoE)

4. **Advanced Caching Strategies**
   - Redis implementation for production
   - Cache warming strategies
   - Cache invalidation patterns

5. **Monitoring and Observability**
   - Application Insights integration
   - Health checks
   - Metrics collection

## 📝 Configuration

### Default appsettings.json 
```json
{
  "ExchangeRateProviders": {
    "CzechNationalBank": {
      "BaseUrl": "https://api.cnb.cz",
      "TimeoutSeconds": 30,
      "Cache": {
        "DailyRatesAbsoluteExpirationInMinutes": 30,
        "DailyRatesSlidingExpirationInMinutes": 10,
        "MonthlyRatesAbsoluteExpirationInMinutes": 1440,
        "MonthlyRatesSlidingExpirationInMinutes": 60
      }
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft": "Warning",
      "ExchangeRateUpdater": "Warning"
    }
  },
  "OpenTelemetry": {
    "Enabled": true,
    "ServiceName": "ExchangeRateUpdater",
    "ServiceVersion": "1.0.0",
    "Tracing": {
      "Enabled": false,
      "ConsoleExporter": {
        "Enabled": false
      },
      "HttpClientInstrumentation": {
        "Enabled": false
      }
    },
    "Logging": {
      "Enabled": true,
      "ConsoleExporter": {
        "Enabled": true
      }
    }
  }
}
```

### Development Configuration
For development and debugging, use this configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "ExchangeRateUpdater": "Information"
    }
  },
  "OpenTelemetry": {
    "Tracing": {
      "Enabled": true,
      "ConsoleExporter": {
        "Enabled": true
      },
      "HttpClientInstrumentation": {
        "Enabled": true
      }
    }
  }
}
```