# InfrastructureLayer

## Overview

The **InfrastructureLayer** is a critical component of the Exchange Rate Updater system that implements the infrastructure concerns and bridges the gap between the **DomainLayer** (business logic) and **DataLayer** (database access) following Clean Architecture principles.

This layer is responsible for:
- Adapting DataLayer repositories to DomainLayer interfaces
- Implementing external service integrations
- Managing background jobs with Hangfire
- Providing cross-cutting infrastructure concerns (date/time, configuration)

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     ApplicationLayer                         │
│                 (CQRS Commands/Queries)                      │
└────────────────────────┬────────────────────────────────────┘
                         │ depends on
                         ↓
┌─────────────────────────────────────────────────────────────┐
│                      DomainLayer                             │
│           (Aggregates, Interfaces, Domain Logic)            │
└────────────────────────┬────────────────────────────────────┘
                         ↑ implemented by
                         │
┌─────────────────────────────────────────────────────────────┐
│                  InfrastructureLayer                         │
│  ┌──────────────────────────────────────────────────────┐   │
│  │ Persistence                                          │   │
│  │  - DomainUnitOfWork (IUnitOfWork implementation)    │   │
│  │  - Repository Adapters (Entity ↔ Aggregate mapping) │   │
│  └──────────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────────┐   │
│  │ ExternalServices                                     │   │
│  │  - DateTimeProvider (IDateTimeProvider)             │   │
│  │  - ProviderDiscoveryService                         │   │
│  └──────────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────────┐   │
│  │ BackgroundJobs                                       │   │
│  │  - Hangfire job implementations                     │   │
│  │  - Job scheduling and management                    │   │
│  └──────────────────────────────────────────────────────┘   │
└────────────────────────┬────────────────────────────────────┘
                         │ uses
                         ↓
┌─────────────────────────────────────────────────────────────┐
│                       DataLayer                              │
│        (EF Core DbContext, Repositories, Entities)          │
└─────────────────────────────────────────────────────────────┘
```

## Key Design Patterns

### 1. Adapter Pattern
Repository adapters bridge DataLayer repositories (entities) to DomainLayer interfaces (aggregates), maintaining Clean Architecture separation.

### 2. Unit of Work Pattern
`DomainUnitOfWork` coordinates repository operations and manages database transactions.

### 3. Dependency Injection
All services are registered via extension methods for clean IoC container configuration.

### 4. Factory Pattern
Domain aggregates use `Reconstruct` factory methods for proper hydration from persistence without validation.

## Project Structure

```
InfrastructureLayer/
├── Persistence/
│   ├── DomainUnitOfWork.cs                          # IUnitOfWork implementation
│   └── Adapters/
│       ├── CurrencyRepositoryAdapter.cs             # Currency mapping
│       ├── ExchangeRateRepositoryAdapter.cs         # ExchangeRate mapping
│       ├── ExchangeRateProviderRepositoryAdapter.cs # Provider mapping
│       ├── UserRepositoryAdapter.cs                 # User mapping
│       ├── FetchLogRepositoryAdapter.cs             # Fetch log mapping (read-only)
│       └── ErrorLogRepositoryAdapter.cs             # Error log mapping (read-only)
├── ExternalServices/
│   ├── DateTimeProvider.cs                          # IDateTimeProvider implementation
│   ├── Adapters/
│   │   ├── IExchangeRateProviderAdapter.cs          # Provider adapter interface
│   │   ├── ExchangeRateProviderAdapter.cs           # Provider adapter implementation
│   │   └── Models/
│   │       ├── ProviderRate.cs                      # Provider rate DTO
│   │       └── ProviderRateResponse.cs              # Provider response DTO
│   └── Discovery/
│       ├── IProviderDiscoveryService.cs             # Provider discovery interface
│       └── ProviderDiscoveryService.cs              # Provider discovery implementation
├── BackgroundJobs/
│   ├── BackgroundJobService.cs                      # Job orchestration service
│   └── Jobs/
│       ├── FetchHistoricalRatesJob.cs               # One-time historical fetch
│       ├── FetchLatestRatesJob.cs                   # Recurring rate fetch
│       └── RetryFailedFetchesJob.cs                 # Retry failed operations
├── InfrastructureLayerServiceExtensions.cs          # DI registration
└── README.md                                         # This file
```

## Components

### Persistence Layer

#### DomainUnitOfWork
Implements `DomainLayer.Interfaces.Persistence.IUnitOfWork` by adapting `DataLayer.IUnitOfWork`.

**Features:**
- Lazy-initialized repository properties
- Transaction management (Begin, Commit, Rollback)
- Coordinated SaveChanges across all repositories
- Proper disposal of underlying DataLayer UnitOfWork

**Usage:**
```csharp
public class MyCommandHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public async Task Handle(MyCommand command)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var provider = await _unitOfWork.ExchangeRateProviders.GetByCodeAsync("ECB");
            provider.RecordSuccessfulFetch();

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
```

#### Repository Adapters

All adapters follow the same pattern:

1. **Wrap DataLayer repository** - Inject `DataLayer.IUnitOfWork`
2. **Map entities to aggregates** - Use `Aggregate.Reconstruct()` for hydration
3. **Map aggregates to entities** - Convert domain models to persistence models
4. **Implement domain repository interface** - Satisfy DomainLayer contracts

**Example: ExchangeRateRepositoryAdapter**
```csharp
private static ExchangeRate MapToDomain(DataLayer.Entities.ExchangeRate entity)
{
    return ExchangeRate.Reconstruct(
        id: entity.Id,
        providerId: entity.ProviderId,
        baseCurrencyId: entity.BaseCurrencyId,
        targetCurrencyId: entity.TargetCurrencyId,
        multiplier: entity.Multiplier,
        rate: entity.Rate,
        validDate: entity.ValidDate,
        created: entity.Created,
        modified: entity.Modified);
}
```

**Key Features:**
- ✅ No reflection (uses proper factory methods)
- ✅ Type-safe mapping
- ✅ Preserves timestamps and audit fields
- ✅ Handles nullable properties correctly
- ✅ Enum conversion with fallback (UserRole)

### External Services

#### DateTimeProvider
Implements `IDateTimeProvider` for testable, consistent date/time handling.

**Properties:**
- `DateTimeOffset UtcNow` - Current UTC date and time
- `DateOnly Today` - Current UTC date (no time component)

**Benefits:**
- Testability: Mock time in unit tests
- Consistency: Single source of truth for time
- Time zone safety: Always UTC

#### ProviderDiscoveryService
Discovers and manages available exchange rate provider implementations.

**Responsibilities:**
- Auto-discovers provider implementations
- Validates provider configurations
- Provides provider metadata
- Manages provider lifecycle

### Background Jobs

#### Job Architecture
Uses **Hangfire** for reliable, persistent background job processing.

**Job Types:**

1. **FetchHistoricalRatesJob** (One-time)
   - Executes on application startup
   - Fetches historical exchange rates for all providers
   - Runs once to populate initial data

2. **FetchLatestRatesJob** (Recurring)
   - Scheduled per provider based on configuration
   - Fetches latest exchange rates
   - Cron expression configurable (default: daily at 16:00 UTC)
   - Time zone aware scheduling

3. **RetryFailedFetchesJob** (Recurring)
   - Retries failed fetch operations
   - Implements exponential backoff
   - Handles quarantined providers

#### BackgroundJobService
Orchestrates job scheduling and management.

**Features:**
- Job enqueueing
- Recurring job scheduling
- Job cancellation
- Job status monitoring

## Dependency Injection Registration

### Service Lifetimes

| Service | Interface | Lifetime | Rationale |
|---------|-----------|----------|-----------|
| DateTimeProvider | IDateTimeProvider | Singleton | Stateless, no per-request data |
| BackgroundJobService | IBackgroundJobService | Scoped | Per-request job operations |
| ProviderDiscoveryService | IProviderDiscoveryService | Singleton | Provider list doesn't change |
| DomainUnitOfWork | IUnitOfWork | Scoped | Per-request database context |
| Background Jobs | Job classes | Transient | New instance per job execution |

### Configuration

**In `Program.cs` or `Startup.cs`:**

```csharp
// Add InfrastructureLayer services
services.AddInfrastructureLayer(configuration);

// After application is built and database is ready
app.UseInfrastructureLayerBackgroundJobs(app.Services);
```

**In `appsettings.json`:**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;..."
  },
  "SystemConfiguration": {
    "BackgroundJobs": {
      "HangfireWorkerCount": 5
    }
  }
}
```

## Configuration Options

### Hangfire Settings

```csharp
// In InfrastructureLayerServiceExtensions.cs
services.AddHangfire(config =>
{
    config.UseSqlServerStorage(connectionString, new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    });
});

services.AddHangfireServer(options =>
{
    options.WorkerCount = workerCount; // From appsettings.json
});
```

### Provider Job Scheduling

Each provider can have custom scheduling:

- **UpdateTime**: Time in format "HH:mm" (e.g., "16:00")
- **TimeZone**: Time zone identifier (e.g., "CET", "UTC", "EET")
- **CronExpression**: Custom cron expression (overrides UpdateTime)

**Example Provider Configuration:**
```json
{
  "Code": "ECB",
  "UpdateTime": "16:00",
  "TimeZone": "CET"
}
```

## Entity-Aggregate Mapping

### Mapping Strategies

#### 1. Value Objects (Currency)
**DataLayer → DomainLayer:**
```csharp
Currency.FromCode(entity.Code, entity.Id)
```
- Simple, immutable mapping
- No state beyond code and ID

#### 2. Aggregates with Reconstruct (User, ExchangeRate, Provider)
**DataLayer → DomainLayer:**
```csharp
User.Reconstruct(
    id: entity.Id,
    email: entity.Email,
    passwordHash: entity.PasswordHash,
    firstName: entity.FirstName,
    lastName: entity.LastName,
    role: ParseUserRole(entity.Role),
    isActive: entity.IsActive,
    created: entity.Created,
    modified: entity.Modified,
    lastLogin: entity.LastLogin)
```
- Bypasses validation for existing data
- Preserves all state including audit fields
- No domain events raised

#### 3. Read-Only Projections (FetchLog, ErrorLog)
**DataLayer → DomainLayer:**
```csharp
new FetchLogEntry(
    entity.Id,
    entity.ProviderId,
    entity.Provider.Code,
    entity.Provider.Name,
    entity.FetchStarted,
    entity.FetchCompleted,
    entity.Status,
    entity.RatesImported,
    entity.RatesUpdated,
    entity.ErrorMessage,
    entity.DurationMs)
```
- Simple record mapping
- No behavior, just data transfer

### Handling Property Mismatches

**Timestamp Preservation:**
```csharp
public async Task UpdateAsync(Currency currency, CancellationToken cancellationToken)
{
    var existingEntity = await _dataLayerUnitOfWork.Currencies.GetByIdAsync(currency.Id);
    var entity = MapToEntity(currency, existingEntity.Created, existingEntity.IsActive);
    await _dataLayerUnitOfWork.Currencies.UpdateAsync(entity);
}
```

**Enum Conversion with Fallback:**
```csharp
if (!Enum.TryParse<UserRole>(entity.Role, true, out var role))
{
    role = UserRole.Consumer; // Safe fallback
}
```

## Best Practices

### 1. Never Use Reflection for Mapping
❌ **Bad:**
```csharp
var user = (User)Activator.CreateInstance(typeof(User), ...);
SetPrivateProperty(user, "Email", entity.Email);
```

✅ **Good:**
```csharp
var user = User.Reconstruct(
    id: entity.Id,
    email: entity.Email,
    ...);
```

### 2. Preserve Audit Fields
Always preserve `Created`, `Modified`, `IsActive` during updates:
```csharp
var existingEntity = await repository.GetByIdAsync(id);
var entity = MapToEntity(domain, existingEntity.Created);
```

### 3. Handle Nullable Properties
```csharp
lastLogin: entity.LastLogin,           // Already nullable
apiKeyVaultReference: entity.ApiKey,  // Already nullable
```

### 4. Use Proper Service Lifetimes
- Singleton: Stateless services (DateTimeProvider, ProviderDiscovery)
- Scoped: Database operations (UnitOfWork)
- Transient: Jobs, short-lived operations

### 5. Transaction Management
Always wrap multi-operation changes in transactions:
```csharp
await _unitOfWork.BeginTransactionAsync();
try
{
    // Multiple operations
    await _unitOfWork.SaveChangesAsync();
    await _unitOfWork.CommitTransactionAsync();
}
catch
{
    await _unitOfWork.RollbackTransactionAsync();
    throw;
}
```

## Testing

### Unit Testing Adapters

```csharp
[Fact]
public async Task MapToDomain_ValidEntity_ReturnsProperAggregate()
{
    // Arrange
    var entity = new DataLayer.Entities.User
    {
        Id = 1,
        Email = "test@example.com",
        PasswordHash = "hash",
        FirstName = "John",
        LastName = "Doe",
        Role = "Admin",
        IsActive = true,
        Created = DateTimeOffset.UtcNow
    };

    // Act
    var user = UserRepositoryAdapter.MapToDomain(entity);

    // Assert
    Assert.Equal(1, user.Id);
    Assert.Equal("test@example.com", user.Email);
    Assert.Equal(UserRole.Admin, user.Role);
    Assert.True(user.IsActive);
}
```

### Integration Testing with Repository

```csharp
[Fact]
public async Task GetByIdAsync_ExistingUser_ReturnsUser()
{
    // Arrange
    var adapter = new UserRepositoryAdapter(_dataLayerUnitOfWork);

    // Act
    var user = await adapter.GetByIdAsync(1);

    // Assert
    Assert.NotNull(user);
    Assert.Equal("test@example.com", user.Email);
}
```

## Troubleshooting

### Common Issues

#### 1. "Cannot update entity: entity not found"
**Cause:** Attempting to update a non-existent entity
**Solution:** Check if entity exists before updating, or handle not found gracefully

#### 2. "Enum parsing failed" / Invalid UserRole
**Cause:** Database contains invalid role string
**Solution:** Adapter has fallback to `UserRole.Consumer`, but check data integrity

#### 3. "Hangfire SQL Server storage connection failed"
**Cause:** Incorrect connection string or database not initialized
**Solution:** Verify connection string and ensure Hangfire tables are created

#### 4. "Provider not discovered"
**Cause:** Provider not registered in DI container
**Solution:** Ensure provider implementations are registered in host application

## Dependencies

### NuGet Packages
- `Microsoft.Extensions.DependencyInjection.Abstractions` - DI support
- `Microsoft.Extensions.Configuration.Abstractions` - Configuration binding
- `Hangfire.Core` - Background job processing
- `Hangfire.SqlServer` - SQL Server job storage

### Project References
- `DomainLayer` - Domain interfaces and aggregates
- `DataLayer` - EF Core repositories and entities
- `ConfigurationLayer` - System configuration
- `ApplicationLayer.Common` - Shared interfaces

## Version History

### Current Version
- ✅ Removed all reflection-based mapping
- ✅ Added proper `Reconstruct` factory methods
- ✅ Fixed entity property mismatches (User, Currency)
- ✅ Implemented UserRole enum conversion
- ✅ Fixed service provider anti-pattern in Hangfire configuration
- ✅ Proper timestamp preservation in updates

### Future Improvements
- [ ] Add domain event dispatching after SaveChanges
- [ ] Implement optimistic concurrency with row versioning
- [ ] Add bulk operation support for performance
- [ ] Implement caching layer for frequently accessed data
- [ ] Add distributed locking for background jobs
- [ ] Implement retry policies with Polly

## Contributing

When adding new repository adapters:

1. **Create the adapter class** in `Persistence/Adapters/`
2. **Implement the domain repository interface**
3. **Add proper mapping methods:**
   - `MapToDomain` using `Aggregate.Reconstruct()`
   - `MapToEntity` for persistence
4. **Handle property mismatches** with defaults or fetching existing data
5. **Add to DomainUnitOfWork** with lazy initialization
6. **Update this README** with new adapter documentation

## License

This project is part of the Exchange Rate Updater system.

## Contact

For questions or issues related to InfrastructureLayer, please contact the development team.

---

**Last Updated:** 2025-11-06
**Maintainers:** Development Team
