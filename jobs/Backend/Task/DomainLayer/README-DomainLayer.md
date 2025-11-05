# Domain Layer

This layer contains the core business logic and domain models following Domain-Driven Design (DDD) principles.

## Architecture

The Domain Layer is the heart of the application and is:
- **Framework-agnostic**: No dependencies on external frameworks or infrastructure
- **Pure business logic**: Contains only domain rules and behaviors
- **Isolated**: Does not depend on other layers (API, Data, Configuration)

## Structure

```
DomainLayer/
├── Aggregates/
│   └── ProviderAggregate/
│       ├── ExchangeRateProvider.cs       # Aggregate root
│       └── ProviderConfiguration.cs      # Child entity
├── ValueObjects/
│   ├── Currency.cs                       # Currency value object
│   ├── ExchangeRateValue.cs             # Exchange rate with multiplier
│   └── Money.cs                         # Amount + Currency
├── DomainServices/
│   └── ProviderHealthMonitor.cs         # Provider health evaluation
├── Enums/
│   ├── ProviderStatus.cs                # Provider operational status
│   ├── FetchStatus.cs                   # Fetch operation status
│   └── UserRole.cs                      # User role (Admin/Consumer)
└── Exceptions/
    ├── DomainException.cs               # Base domain exception
    ├── ProviderNotActiveException.cs    # Provider not active
    ├── ProviderQuarantinedException.cs  # Provider quarantined
    └── InvalidExchangeRateException.cs  # Invalid exchange rate
```

## Domain Model

### Aggregates

#### ExchangeRateProvider (Aggregate Root)

The central aggregate that manages exchange rate providers.

**Responsibilities:**
- Provider lifecycle management (activate/deactivate)
- Health monitoring (success/failure tracking)
- Configuration management
- Quarantine logic based on consecutive failures

**Key Business Rules:**
- Provider is quarantined after 5 consecutive failures
- Quarantined providers require manual intervention to reactivate
- Provider must be active to fetch rates
- Authentication is required if `RequiresAuthentication` is true

**Domain Methods:**
```csharp
// Lifecycle
void Activate()
void Deactivate()

// Health tracking
void RecordSuccessfulFetch()
void RecordFailedFetch()
void ResetHealthStatus()

// Configuration
void SetConfiguration(string key, string value, string? description = null)
string? GetConfigurationValue(string key)
T? GetConfigurationValueAs<T>(string key)

// Validation
void EnsureCanFetch()  // Throws if provider cannot be used

// Computed properties
ProviderStatus Status { get; }
bool IsHealthy { get; }
bool ShouldBeQuarantined { get; }
TimeSpan? TimeSinceLastSuccessfulFetch { get; }
```

**Example Usage:**
```csharp
// Create a new provider
var provider = ExchangeRateProvider.Create(
    name: "European Central Bank",
    code: "ECB",
    url: "https://api.ecb.europa.eu/rates",
    baseCurrencyId: 1, // EUR
    requiresAuthentication: false
);

// Configure provider
provider.SetConfiguration("MaxRetries", "3");
provider.SetConfiguration("TimeoutSeconds", "30");

// Record operations
provider.RecordSuccessfulFetch();
// or
provider.RecordFailedFetch(); // Increments consecutive failures

// Check health
if (provider.IsHealthy)
{
    provider.EnsureCanFetch(); // Throws if cannot fetch
    // Fetch rates...
}

// Handle quarantine
if (provider.ShouldBeQuarantined)
{
    // Provider is automatically deactivated
    // Requires manual intervention:
    provider.ResetHealthStatus();
    provider.Activate();
}
```

#### ProviderConfiguration (Entity)

Configuration key-value pairs for a provider.

**Features:**
- Type-safe value retrieval
- Automatic timestamp tracking
- Value validation

```csharp
var config = ProviderConfiguration.Create(
    providerId: 1,
    settingKey: "ApiTimeout",
    settingValue: "30000",
    description: "Timeout in milliseconds"
);

// Update value
config.UpdateValue("60000", "Increased timeout");

// Type-safe retrieval
int timeout = config.GetValueAs<int>(); // 60000
```

---

### Value Objects

#### Currency

Immutable representation of a currency using ISO 4217 codes.

**Features:**
- Validation against known currency codes
- Case-insensitive parsing
- Factory methods for safety

```csharp
// Create currency
var usd = Currency.FromCode("USD");
var eur = Currency.EUR; // Predefined

// Validation
if (Currency.IsValid("GBP"))
{
    var gbp = Currency.FromCode("GBP");
}

// Safe creation
if (Currency.TryFromCode("XXX", out var currency))
{
    // Use currency
}
```

#### ExchangeRateValue

Represents an exchange rate with precision handling.

**Why Rate + Multiplier?**
Exchange rates like 1 EUR = 25.50 CZK are stored as:
- Rate = 255
- Multiplier = 10
- Actual Rate = 255 / 10 = 25.5

This avoids floating-point precision errors.

**Features:**
- Precise decimal arithmetic
- Inverse rate calculation
- Amount conversion
- Threshold comparison

```csharp
// Create from decimal
var rate = ExchangeRateValue.FromDecimal(25.50m); // Rate=255000, Multiplier=10000

// Or specify manually
var rate2 = ExchangeRateValue.Create(rate: 255, multiplier: 10);

// Calculate inverse
var inverseRate = rate.Inverse(); // CZK to EUR

// Convert amounts
decimal czk = rate.Convert(100m); // 100 EUR -> 2550 CZK

// Compare rates
bool similar = rate.IsWithinThreshold(rate2, thresholdPercent: 1.0m);

// Operators
if (rate > rate2) { /* ... */ }
```

#### Money

Represents a monetary amount with its currency.

**Features:**
- Amount always paired with currency
- Arithmetic operations (add, subtract, multiply, divide)
- Currency conversion
- Rounding support

```csharp
// Create money
var money = Money.Create(100m, Currency.USD);

// Arithmetic (same currency only)
var total = money.Add(Money.Create(50m, Currency.USD)); // 150 USD
var doubled = money.Multiply(2); // 200 USD

// Currency conversion
var exchangeRate = ExchangeRateValue.FromDecimal(0.85m); // USD to EUR
var euros = money.ConvertTo(Currency.EUR, exchangeRate); // 85 EUR

// Rounding
var rounded = money.Round(2); // Round to 2 decimal places

// Operators
var sum = money + Money.Create(25m, Currency.USD);
var product = money * 1.5m;

// Comparison (same currency only)
if (money > Money.Create(50m, Currency.USD))
{
    // money is greater
}
```

---

### Domain Services

#### ProviderHealthMonitor

Domain service for evaluating and monitoring provider health.

**Responsibilities:**
- Health status evaluation
- Stale data detection
- Auto-recovery eligibility
- Health recommendations

```csharp
var monitor = new ProviderHealthMonitor(
    staleDataThreshold: TimeSpan.FromHours(24),
    criticalFailureThreshold: 3
);

// Evaluate single provider
var healthStatus = monitor.EvaluateHealth(provider);

Console.WriteLine($"Health Level: {healthStatus.HealthLevel}");
Console.WriteLine($"Is Healthy: {healthStatus.IsHealthy}");
Console.WriteLine($"Is Data Stale: {healthStatus.IsDataStale}");
Console.WriteLine($"Recommendations:");
foreach (var recommendation in healthStatus.Recommendations)
{
    Console.WriteLine($"  - {recommendation}");
}

// Evaluate all providers
var providers = await GetAllProviders();
var summary = monitor.EvaluateHealthForAll(providers);

Console.WriteLine($"Total: {summary.TotalProviders}");
Console.WriteLine($"Healthy: {summary.HealthyProviders}");
Console.WriteLine($"Unhealthy: {summary.UnhealthyProviders}");
Console.WriteLine($"Quarantined: {summary.QuarantinedProviders}");
Console.WriteLine($"Overall Health: {summary.OverallHealth:F2}%");

// Check auto-recovery eligibility
if (monitor.CanAutoRecover(provider, TimeSpan.FromHours(2)))
{
    provider.ResetHealthStatus();
    provider.Activate();
}
```

---

### Enums

#### ProviderStatus
- `Active`: Operational and can fetch rates
- `Inactive`: Temporarily disabled
- `Quarantined`: Too many failures, requires manual intervention
- `Pending`: Configuration incomplete

#### FetchStatus
- `Running`: Operation in progress
- `Success`: Completed successfully
- `Failed`: Completely failed
- `PartialSuccess`: Some rates imported, some errors

#### UserRole
- `Consumer`: Can view exchange rates and use the API
- `Admin`: Full access to manage providers, view logs, and system configuration

#### HealthLevel (from ProviderHealthMonitor)
- `Unknown`: No fetch history
- `Healthy`: Working normally
- `Warning`: Minor issues (1-2 failures, stale data)
- `Critical`: Major issues (3+ failures)
- `Inactive`: Provider disabled

---

### Exceptions

All domain exceptions inherit from `DomainException`.

#### ProviderNotActiveException
Thrown when attempting to use an inactive provider.

```csharp
try
{
    provider.EnsureCanFetch();
}
catch (ProviderNotActiveException ex)
{
    Console.WriteLine($"Provider {ex.ProviderCode} is {ex.CurrentStatus}");
}
```

#### ProviderQuarantinedException
Thrown when a provider is quarantined.

```csharp
catch (ProviderQuarantinedException ex)
{
    Console.WriteLine($"Provider {ex.ProviderCode} quarantined after {ex.ConsecutiveFailures} failures");
    // Notify administrators
}
```

#### InvalidExchangeRateException
Thrown for invalid exchange rate data.

```csharp
catch (InvalidExchangeRateException ex)
{
    Console.WriteLine($"Invalid rate: {ex.BaseCurrency} -> {ex.TargetCurrency}");
}
```

---

## Design Decisions

### Why No ExchangeRate Domain Entity?

The `ExchangeRate` table has validation constraints but limited business behavior:
- Constraints are enforced at database level
- No complex state transitions
- No invariants to protect
- Mostly a data container

**Decision**: Use data models with validation attributes in DataLayer instead of domain entities.

If future requirements add business logic (e.g., rate arbitrage detection, automatic rate corrections), we can promote it to a domain entity.

### Why Currency as Value Object?

Currencies:
- Are immutable
- Have no identity beyond their code
- Are shared across multiple contexts
- Perfect fit for value object pattern

### Why Separate ProviderConfiguration?

While ProviderConfiguration is part of the Provider aggregate, it's a separate entity because:
- Has its own lifecycle (created, updated independently)
- Has its own identity (Id)
- Can be queried separately
- Follows DDD entity pattern within aggregate

---

## Best Practices

### 1. Always Use Factory Methods

```csharp
// ✅ Good
var provider = ExchangeRateProvider.Create(...);

// ❌ Bad (constructor is private)
var provider = new ExchangeRateProvider(...); // Won't compile
```

### 2. Encapsulation

Domain objects protect their invariants:

```csharp
// ✅ Good - uses domain method
provider.RecordFailedFetch(); // Increments failures, updates timestamp

// ❌ Bad - if we could do this
provider.ConsecutiveFailures++; // Bypasses business logic
```

### 3. Rich Domain Model

Put behavior where the data is:

```csharp
// ✅ Good
if (provider.ShouldBeQuarantined)
{
    // Handle quarantine
}

// ❌ Bad
if (provider.ConsecutiveFailures >= 5)
{
    // Business rule leaked into application layer
}
```

### 4. Value Object Immutability

```csharp
// ✅ Good
var newRate = rate.Inverse();

// Value objects are immutable - operations return new instances
```

### 5. Aggregate Boundaries

Only access child entities through the aggregate root:

```csharp
// ✅ Good
provider.SetConfiguration("key", "value");

// ❌ Bad
var config = new ProviderConfiguration(...);
dbContext.Add(config); // Bypasses aggregate root
```

---

## Testing Domain Logic

The domain layer is highly testable because it has no external dependencies:

```csharp
[Fact]
public void Provider_Should_Be_Quarantined_After_Five_Failures()
{
    // Arrange
    var provider = ExchangeRateProvider.Create("Test", "TEST", "http://test", 1);

    // Act
    for (int i = 0; i < 5; i++)
    {
        provider.RecordFailedFetch();
    }

    // Assert
    Assert.Equal(ProviderStatus.Quarantined, provider.Status);
    Assert.False(provider.IsActive);
    Assert.True(provider.ShouldBeQuarantined);
}

[Fact]
public void Money_Cannot_Add_Different_Currencies()
{
    // Arrange
    var usd = Money.Create(100, Currency.USD);
    var eur = Money.Create(100, Currency.EUR);

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => usd.Add(eur));
}
```

---

## Future Enhancements

Potential additions as business requirements evolve:

1. **ExchangeRate Domain Entity** - If rate validation/calculation logic becomes complex
2. **CurrencyPair Value Object** - For representing currency pairs with swap logic
3. **RateArbitrageDetector Domain Service** - Detect arbitrage opportunities
4. **ProviderPriority** - Prioritize providers based on reliability
5. **Domain Events** - Publish events for provider quarantine, successful fetch, etc.

---

## References

- [Domain-Driven Design by Eric Evans](https://www.domainlanguage.com/ddd/)
- [Implementing Domain-Driven Design by Vaughn Vernon](https://vaughnvernon.com/)
- [Value Objects in DDD](https://martinfowler.com/bliki/ValueObject.html)
- [Aggregate Pattern](https://martinfowler.com/bliki/DDD_Aggregate.html)
