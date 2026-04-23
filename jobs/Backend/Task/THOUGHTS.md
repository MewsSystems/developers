# David's Notes: Exchange Rate Provider Implementation
## Overview
**My personal Philosophy** is that: Above all design and complexities, write **READABLE & UNDERSTANDABLE** code for others and and my future self.

The code is almost ready for production with minor enhancements (health checks, caching...). It balances simplicity with robustness, avoiding over-engineering while maintaining standards.

---

## Decisions

### 1. API Integration Over Text File Parsing

**Decision**: Use CNB's JSON API (`https://api.cnb.cz/cnbapi/exrates/daily`) instead of parsing their text file format.

**Logic**:
- **Reliability**: APIs have versioning and backward compatibility guarantees; text formats can change without notice
- **Structured Data**: JSON deserializes directly into strongly-typed objects, eliminating parsing errors
- **Future-Proofing**: If CNB deprecates the text format, the API will likely receive advance notice

**Trade-offs**:
- Text files might be faster (no HTTP overhead)
- But APIs win on reliability and maintainability in production

---

### 2. Dependency Injection Pattern

**Implementation**:
```csharp
services.AddHttpClient<CNBExchangeRateApiProvider>();
services.AddTransient<IExchangeRateProvider, CNBExchangeRateApiProvider>();
```

**Logic**:
- **Testability**: Allows mocking `HttpClient` and `ILogger` in unit tests
- **Flexibility**: Easy to swap implementations (e.g., add a database-backed provider, caching...)
- **Configuration Injection**: `IConfiguration` can be injected and tested with in-memory providers

---

### 3. Resilience: Polly Retry Policy

**Logic**:
- **Transient Failures**: Network hiccups, temporary API outages, rate limiting
- **Exponential Backoff**: 2s, 4s, 8s delays prevent overwhelming a struggling service
- **Production Reality**: External APIs fail;

---

### 4. Configuration Externalization

**Implementation**: `appsettings.json` with strongly-typed configuration access.

**Logic**:
- **Environment Parity**: Different configs for Dev/Test/Prod without code changes
- **Security**: Secrets can be replaced with environment variables or Azure Key Vault

---

### 5. Logging Strategy

**Logic**:
- **Operations**: Info logs show system health; errors trigger alerts
- **Debugging**: Debug logs help troubleshoot production issues without redeploying

---

## Testing Strategy

### Why Unit Tests?

**Coverage**:
- Happy path (valid API response)
- Edge cases (empty input, normalization)
- Error scenarios (HTTP errors, invalid JSON)
- Behavioral testing (ignores target currency, filters requested currencies)

### Why Mock HttpClient?

Testing against a real API is:
- **Unreliable**: API might be down
- **Non-Deterministic**: Results change daily

Mocking gives:
- **Speed**: Millisecond execution
- **Reliability**: 100% deterministic
- **Control**: Test error scenarios without breaking things

---

## What Would I Add for a Real Production System?

This is a simple but reliable version of an exchange rate provider. For a real production system with more time, I may add:
1. Health Checks
2. Caching
3. Observability (Metrics & Tracing)

---

## Technology Choices

### Why .NET 10?
- **Latest LTS**: Long-term support, latest performance improvements
- **Performance**: .NET continues to improve runtime performance

### Why Polly?
- **Standard**: De facto resilience library for .NET

### Why System.Text.Json?
- **Performance**: Faster than Newtonsoft.Json
- **Built-in**: Built-in to .NET, actively maintained

---

## Conclusion

This implementation demonstrates:
- **Clean Architecture**: Separation of concerns, dependency injection
- **Resilience**: Retry logic, graceful degradation
- **Testability**: Mocked dependencies, comprehensive test coverage
- **Production Mindset**: Logging, configuration, error handling