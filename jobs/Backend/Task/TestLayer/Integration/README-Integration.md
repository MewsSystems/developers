# Integration Tests - ApplicationLayer

This directory contains integration tests for the ApplicationLayer commands and queries, testing the full application stack with a real database.

## Testing Approach

### Real Dependencies Strategy
We test with **real database and actual dependencies** to verify the complete flow:

```
Test → ApplicationLayer → InfrastructureLayer → DataLayer → Real Database
```

This approach:
- ✅ Tests the complete application flow end-to-end
- ✅ Verifies actual database operations (SQL Server LocalDB)
- ✅ Tests EF Core mappings and migrations
- ✅ Tests repository implementations and adapters
- ✅ Validates database views and stored procedures
- ✅ Catches integration issues that unit tests miss
- ✅ Uses Respawn for fast database cleanup between tests

### Test Structure

```
TestLayer/Integration/
├── Infrastructure/
│   ├── IntegrationTestBase.cs          # Base class with database setup
│   └── FakeBackgroundJobService.cs     # Test implementation for Hangfire
├── Commands/
│   ├── Currencies/                     # Currency command tests
│   ├── ExchangeRates/                  # Exchange rate command tests
│   ├── ExchangeRateProviders/          # Provider command tests
│   └── Users/                          # User command tests
└── Queries/
    ├── Currencies/                     # Currency query tests
    ├── ExchangeRates/                  # Exchange rate query tests
    ├── Providers/                      # Provider query tests
    └── Users/                          # User query tests
```

## IntegrationTestBase Class

All tests inherit from `IntegrationTestBase` which provides:

### Real Dependencies
- **ApplicationDbContext** - Real EF Core DbContext
- **DomainLayer UnitOfWork** - Real domain layer unit of work
- **DataLayer Repositories** - Real repository implementations
- **InfrastructureLayer Adapters** - Real adapter implementations
- **MediatR** - Real mediator for CQRS
- **SQL Server LocalDB** - Real test database
- **Respawn** - Fast database cleanup between tests

### Database Configuration
```csharp
private const string ConnectionString =
    "Server=(localdb)\\MSSQLLocalDB;" +
    "Database=ExchangeRateUpdaterTest;" +
    "Integrated Security=true;" +
    "Connection Timeout=30;" +
    "MultipleActiveResultSets=true;";
```

### Lifecycle Methods
- `InitializeAsync()` - Called before each test
  - Ensures database is created
  - Resets database to clean state using Respawn
- `DisposeAsync()` - Called after each test
  - Cleans up resources

### Registered Services
All services are registered exactly as in production:
```csharp
services.AddDbContext<ApplicationDbContext>()
services.AddScoped<DataLayer.IUnitOfWork, DataLayer.UnitOfWork>()
services.AddScoped<DomainLayer.Interfaces.Persistence.IUnitOfWork, InfrastructureLayer.Persistence.DomainUnitOfWork>()
services.AddApplicationLayer()
```

## Testing Commands

Commands are operations that **change state** (Create, Update, Delete).

### Example: CreateExchangeRateProviderCommand

```csharp
public class CreateExchangeRateProviderCommandTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateProvider_WithValidData_ShouldPersistToDatabase()
    {
        // Arrange - Create currency first
        var currencyCommand = new CreateCurrencyCommand("USD");
        var currencyResult = await Mediator.Send(currencyCommand);
        var currencyId = currencyResult.Value;

        var command = new CreateExchangeRateProviderCommand(
            Name: "European Central Bank",
            Code: "ECB",
            Url: "https://api.ecb.europa.eu/rates",
            BaseCurrencyId: currencyId,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert - Verify command result
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);

        // Assert - Verify actual database state
        var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
            .FirstOrDefaultAsync(p => p.Id == result.Value);

        provider.Should().NotBeNull();
        provider!.Code.Should().Be("ECB");
        provider.Name.Should().Be("European Central Bank");
        provider.BaseCurrencyId.Should().Be(currencyId);
        provider.IsActive.Should().BeTrue();
        provider.ConsecutiveFailures.Should().Be(0);
    }
}
```

### Test Scenarios for Commands
1. ✅ **Success Case** - Valid input, verify database persistence
2. ✅ **Validation Failures** - Invalid input rejected before database
3. ✅ **Business Rule Violations** - E.g., duplicate entity in database
4. ✅ **Database Constraints** - Foreign keys, unique constraints
5. ✅ **Transaction Rollback** - Errors don't leave partial data
6. ✅ **Multiple Operations** - Test sequences of related commands
7. ✅ **Domain Events** - Verify events are dispatched

### Helper Pattern
Create helper methods to set up test data:

```csharp
private async Task<int> CreateTestCurrencyAsync(string code = "USD")
{
    var command = new CreateCurrencyCommand(code);
    var result = await Mediator.Send(command);
    result.IsSuccess.Should().BeTrue();
    return result.Value;
}

private async Task<int> CreateTestProviderAsync(int currencyId, string? code = null)
{
    code ??= Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);
    var command = new CreateExchangeRateProviderCommand(
        Name: "Test Provider",
        Code: code,
        Url: "https://api.example.com/rates",
        BaseCurrencyId: currencyId,
        RequiresAuthentication: false,
        ApiKeyVaultReference: null
    );
    var result = await Mediator.Send(command);
    result.IsSuccess.Should().BeTrue();
    return result.Value;
}
```

## Testing Queries

Queries are operations that **read data** from the database.

### Example: GetAllProvidersQuery

```csharp
public class GetAllProvidersQueryTests : IntegrationTestBase
{
    [Fact]
    public async Task GetAllProviders_WithMultipleProviders_ShouldReturnAll()
    {
        // Arrange - Create test data in database
        var usdId = await CreateTestCurrencyAsync("USD");
        var eurId = await CreateTestCurrencyAsync("EUR");

        await CreateTestProviderAsync(usdId, "ECB", "European Central Bank");
        await CreateTestProviderAsync(eurId, "CNB", "Czech National Bank");
        await CreateTestProviderAsync(usdId, "BNR", "Romanian National Bank");

        // Act - Query the data
        var query = new GetAllProvidersQuery(PageNumber: 1, PageSize: 10);
        var result = await Mediator.Send(query);

        // Assert - Verify query results
        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
        result.Items.Should().Contain(p => p.Code == "ECB");
        result.Items.Should().Contain(p => p.Code == "CNB");
        result.Items.Should().Contain(p => p.Code == "BNR");
    }

    [Fact]
    public async Task GetAllProviders_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange - Create 5 providers
        var usdId = await CreateTestCurrencyAsync("USD");
        for (int i = 1; i <= 5; i++)
        {
            await CreateTestProviderAsync(usdId, $"PROV{i}", $"Provider {i}");
        }

        // Act - Get page 2 with 2 items per page
        var query = new GetAllProvidersQuery(PageNumber: 2, PageSize: 2);
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(5);
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(2);
        result.TotalPages.Should().Be(3); // 5 items / 2 per page = 3 pages
    }

    [Fact]
    public async Task GetAllProviders_FilterByIsActive_ShouldReturnOnlyActive()
    {
        // Arrange - Create active and inactive providers
        var usdId = await CreateTestCurrencyAsync("USD");

        var activeId = await CreateTestProviderAsync(usdId, "ACTIVE", "Active One");
        var inactiveId = await CreateTestProviderAsync(usdId, "INACTIVE", "Inactive One");

        // Deactivate one provider
        await Mediator.Send(new DeactivateProviderCommand(inactiveId));

        // Act - Filter for active only
        var query = new GetAllProvidersQuery(IsActive: true);
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.Should().OnlyContain(p => p.IsActive);
        result.Items.First().Code.Should().Be("ACTIVE");
    }
}
```

### Test Scenarios for Queries
1. ✅ **With Data** - Returns correct data from database
2. ✅ **Empty Results** - Returns empty list when no data
3. ✅ **Pagination** - Correct page size, total count, page number
4. ✅ **Filtering** - Query filters work with database
5. ✅ **Sorting** - Results ordered correctly
6. ✅ **Joins** - Data from multiple tables combined correctly
7. ✅ **Database Views** - Views return expected data
8. ✅ **Search/Text Filtering** - Text searches work correctly

## Testing Database Views

Many queries use optimized database views. Test them thoroughly:

```csharp
[Fact]
public async Task GetProviderHealth_WithValidId_ShouldReturnHealthFromView()
{
    // Arrange - Create provider
    var currencyId = await CreateTestCurrencyAsync("USD");
    var providerId = await CreateTestProviderAsync(currencyId);

    // Act - Query uses vw_ProviderHealthStatus view
    var query = new GetProviderHealthQuery(providerId);
    var result = await Mediator.Send(query);

    // Assert - View calculates health status correctly
    result.Should().NotBeNull();
    result!.ProviderId.Should().Be(providerId);
    result.ConsecutiveFailures.Should().Be(0);
    // New providers have never fetched, so not considered "Healthy"
    result.IsHealthy.Should().BeFalse();
    result.Status.Should().Be("Never Fetched");
    result.LastSuccessfulFetch.Should().BeNull();
}
```

## Best Practices

### 1. Arrange-Act-Assert Pattern
Always use clear AAA structure with real database operations:
```csharp
// Arrange - Create real data in database
var currencyId = await CreateTestCurrencyAsync("USD");
var providerId = await CreateTestProviderAsync(currencyId);

// Act - Execute command/query
var result = await Mediator.Send(command);

// Assert - Verify both result AND database state
result.IsSuccess.Should().BeTrue();
var dbEntity = await DbContext.Set<Entity>().FindAsync(result.Value);
dbEntity.Should().NotBeNull();
```

### 2. Verify Database State
Always verify actual database changes, not just command results:
```csharp
// ✅ Good - Verifies actual database state
var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
    .FirstOrDefaultAsync(p => p.Id == providerId);
provider.Should().NotBeNull();
provider!.IsActive.Should().BeFalse();

// ❌ Insufficient - Only checks command result
result.IsSuccess.Should().BeTrue();
```

### 3. Test Database Constraints
Verify that database constraints are enforced:
```csharp
[Fact]
public async Task CreateProvider_WithDuplicateCode_ShouldFail()
{
    // Arrange
    var currencyId = await CreateTestCurrencyAsync("USD");
    await CreateTestProviderAsync(currencyId, "ECB");

    // Act - Try to create duplicate
    var command = new CreateExchangeRateProviderCommand(
        "Duplicate", "ECB", "https://dup.com", currencyId, false, null);
    var result = await Mediator.Send(command);

    // Assert
    result.IsSuccess.Should().BeFalse();
    result.Error.Should().Contain("already exists");
}
```

### 4. Use Unique Codes
Avoid test collisions by using unique codes:
```csharp
// ✅ Good - Generates unique code
var code = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);

// ❌ Avoid - Hard-coded codes can collide
var code = "TEST123";
```

### 5. Clean Test Data Pattern
Use helper methods to create clean test data:
```csharp
private async Task<int> CreateTestCurrencyAsync(string code)
{
    var command = new CreateCurrencyCommand(code);
    var result = await Mediator.Send(command);
    result.IsSuccess.Should().BeTrue();
    return result.Value;
}
```

### 6. Test Sequences of Operations
Integration tests excel at testing workflows:
```csharp
[Fact]
public async Task ProviderLifecycle_CreateActivateDeactivate_ShouldWork()
{
    // Create
    var currencyId = await CreateTestCurrencyAsync("USD");
    var providerId = await CreateTestProviderAsync(currencyId);

    // Verify created and active by default
    var provider = await DbContext.Set<DataLayer.Entities.ExchangeRateProvider>()
        .FindAsync(providerId);
    provider!.IsActive.Should().BeTrue();

    // Deactivate
    var deactivateResult = await Mediator.Send(new DeactivateProviderCommand(providerId));
    deactivateResult.IsSuccess.Should().BeTrue();

    // Verify deactivated
    await DbContext.Entry(provider).ReloadAsync();
    provider.IsActive.Should().BeFalse();

    // Reactivate
    var activateResult = await Mediator.Send(new ActivateProviderCommand(providerId));
    activateResult.IsSuccess.Should().BeTrue();

    // Verify active again
    await DbContext.Entry(provider).ReloadAsync();
    provider.IsActive.Should().BeTrue();
}
```

### 7. Test Database Transactions
Verify transaction behavior:
```csharp
[Fact]
public async Task Command_WithException_ShouldRollbackTransaction()
{
    // This would require intentionally causing an exception
    // Verify that no partial data is left in the database
}
```

### 8. Name Tests Descriptively
Use clear naming: `MethodName_Scenario_ExpectedBehavior`
```csharp
CreateProvider_WithValidData_ShouldPersistToDatabase
GetAllProviders_WithPagination_ShouldReturnCorrectPage
UpdateProvider_WithNonExistentId_ShouldFail
```

### 9. Test Error Messages
Verify that user-facing errors are clear:
```csharp
result.IsSuccess.Should().BeFalse();
result.Error.Should().Contain("not found");
```

### 10. Understand Database Cleanup
Respawn cleans all tables between tests automatically. Each test starts with a clean database.

## Running Tests

### Run all integration tests:
```bash
cd TestLayer/Integration
dotnet test
```

### Run specific test class:
```bash
dotnet test --filter "FullyQualifiedName~CreateExchangeRateProviderCommandTests"
```

### Run specific test:
```bash
dotnet test --filter "FullyQualifiedName~CreateProvider_WithValidData_ShouldPersistToDatabase"
```

### Run tests for specific domain:
```bash
# Provider tests only
dotnet test --filter "FullyQualifiedName~Integration.Commands.ExchangeRateProviders|FullyQualifiedName~Integration.Queries.Providers"

# Currency tests only
dotnet test --filter "FullyQualifiedName~Currencies"

# Exchange rate tests only
dotnet test --filter "FullyQualifiedName~ExchangeRates"
```

### Run with verbose output:
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Run with minimal output:
```bash
dotnet test --logger "console;verbosity=minimal"
```

## Database Setup

### Prerequisites
- **SQL Server LocalDB** must be installed
- Available with Visual Studio or SQL Server Express
- Test database: `ExchangeRateUpdaterTest`

### Verify LocalDB:
```bash
sqllocaldb info
```

### Create/Update Database:
The database is created automatically on first test run via:
```csharp
await DbContext.Database.EnsureCreatedAsync();
```

### Manual Database Inspection:
```bash
# Connect to LocalDB
sqlcmd -S "(localdb)\MSSQLLocalDB" -d ExchangeRateUpdaterTest

# View tables
SELECT * FROM sys.tables;

# Query data
SELECT * FROM ExchangeRateProvider;
SELECT * FROM Currency;
```

## Dependencies

- **xUnit** - Test framework
- **FluentAssertions** - Assertion library
- **MediatR** - CQRS mediator
- **Entity Framework Core** - ORM
- **SQL Server LocalDB** - Test database
- **Respawn** - Fast database cleanup
- **Dapper** - For view queries
- **Microsoft.Extensions.DependencyInjection** - DI container

## Test Coverage Goals

Aim for:
- ✅ **100% Coverage** of all commands and queries
- ✅ **All CRUD operations** tested
- ✅ **All database views** tested
- ✅ **All business rules** verified with real database
- ✅ **Pagination, filtering, sorting** tested
- ✅ **Database constraints** validated
- ✅ **Happy paths and error paths** covered

## Current Test Statistics

**Total Integration Tests: 210**

### By Domain:
- **Currencies**: ~50 tests
  - Commands: CreateCurrency
  - Queries: GetAllCurrencies, GetCurrencyById, GetCurrencyByCode

- **Exchange Rates**: ~90 tests
  - Commands: BulkUpsertExchangeRates
  - Queries: GetCurrentRates, GetHistoricalRates, etc.

- **Providers**: 60 tests
  - Commands (28 tests):
    - CreateExchangeRateProvider (6 tests)
    - ActivateProvider (4 tests)
    - DeactivateProvider (4 tests)
    - ResetProviderHealth (4 tests)
    - UpdateProviderConfiguration (6 tests)
    - TriggerManualFetch (4 tests)
  - Queries (32 tests):
    - GetAllProviders (8 tests)
    - GetProviderById (6 tests)
    - GetProviderConfiguration (4 tests)
    - GetProviderHealth (4 tests)
    - GetProviderStatistics (6 tests)

- **Users**: ~10 tests
  - Commands: CreateUser, ChangeUserRole, DeleteUser
  - Queries: GetAllUsers, GetUserById, GetUserByEmail, GetUsersByRole

## Common Issues and Solutions

### Issue: Tests fail with database connection error
**Solution**: Ensure SQL Server LocalDB is installed and running:
```bash
sqllocaldb start MSSQLLocalDB
```

### Issue: Tests fail with "table already exists"
**Solution**: Respawn should handle this automatically. If not, manually drop the test database:
```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "DROP DATABASE ExchangeRateUpdaterTest"
```

### Issue: Tests are slow
**Solution**:
- Respawn is very fast (resets DB in ~50ms)
- If still slow, check for missing indexes on test database
- Consider running fewer tests in parallel

### Issue: Unique constraint violation
**Solution**: Use unique codes in each test:
```csharp
var code = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);
```

### Issue: Foreign key constraint error
**Solution**: Always create dependent entities first:
```csharp
// ✅ Correct order
var currencyId = await CreateTestCurrencyAsync("USD");
var providerId = await CreateTestProviderAsync(currencyId);

// ❌ Wrong - FK violation
var providerId = await CreateTestProviderAsync(999); // Currency 999 doesn't exist
```

## Adding New Tests

1. Create test class inheriting from `IntegrationTestBase`
2. Add helper methods for creating test data
3. Write tests following AAA pattern
4. Use FluentAssertions for assertions
5. Verify database state, not just command results
6. Run tests and ensure they pass
7. Verify database is clean after test (Respawn handles this)

## Example Test Class Template

```csharp
using ApplicationLayer.Commands.YourDomain.YourCommand;
using ApplicationLayer.Queries.YourDomain.YourQuery;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.YourDomain;

public class YourCommandTests : IntegrationTestBase
{
    // Helper methods to create test data
    private async Task<int> CreateTestDataAsync()
    {
        // Create necessary test data
        var command = new CreateSomethingCommand(...);
        var result = await Mediator.Send(command);
        result.IsSuccess.Should().BeTrue();
        return result.Value;
    }

    [Fact]
    public async Task YourCommand_WithValidData_ShouldPersistToDatabase()
    {
        // Arrange
        var testData = await CreateTestDataAsync();
        var command = new YourCommand(...);

        // Act
        var result = await Mediator.Send(command);

        // Assert - Verify command result
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);

        // Assert - Verify database state
        var entity = await DbContext.Set<YourEntity>()
            .FirstOrDefaultAsync(e => e.Id == result.Value);

        entity.Should().NotBeNull();
        entity!.SomeProperty.Should().Be("ExpectedValue");
    }

    [Fact]
    public async Task YourQuery_WithData_ShouldReturnResults()
    {
        // Arrange - Create test data
        await CreateTestDataAsync();

        // Act
        var query = new YourQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeEmpty();
        result.First().SomeProperty.Should().Be("ExpectedValue");
    }

    [Fact]
    public async Task YourCommand_WithInvalidData_ShouldFail()
    {
        // Arrange
        var command = new YourCommand(invalidParameter);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("expected error message");
    }
}
```

## Integration vs Unit Tests

### When to use Integration Tests:
- ✅ Testing complete workflows
- ✅ Verifying database operations
- ✅ Testing EF Core mappings
- ✅ Validating database views/stored procedures
- ✅ Testing database constraints
- ✅ Verifying transactions
- ✅ Testing data transformations through all layers

### When to use Unit Tests:
- ✅ Testing business logic in isolation
- ✅ Testing edge cases with mocked data
- ✅ Fast feedback during development
- ✅ Testing error handling paths
- ✅ Testing without database dependencies

**Best Practice**: Use both! Unit tests for fast feedback and edge cases. Integration tests for confidence in the complete system.

## Additional Resources

- [xUnit Documentation](https://xunit.net/)
- [FluentAssertions Documentation](https://fluentassertions.com/introduction)
- [Respawn GitHub](https://github.com/jbogard/Respawn)
- [Integration Testing Best Practices](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests)
- [EF Core Testing](https://learn.microsoft.com/en-us/ef/core/testing/)
