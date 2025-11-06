# Unit Tests - ApplicationLayer

This directory contains unit tests for the ApplicationLayer commands and queries.

## Testing Approach

### Mocking Strategy
We mock **only at the DataLayer boundary** while testing the full flow through ApplicationLayer:

```
Test → ApplicationLayer Handler → [MOCK] DataLayer Repositories/Views
```

This approach:
- ✅ Tests the actual command/query handler logic
- ✅ Tests data flow and transformations
- ✅ Tests error handling and validation
- ✅ Mocks only database interactions
- ✅ Allows testing without a real database

### Test Structure

```
TestLayer/Unit/
├── ApplicationLayer/
│   ├── TestBase.cs                    # Base class with common mocking setup
│   ├── Commands/
│   │   └── *CommandHandlerTests.cs    # Command handler tests
│   └── Queries/
│       └── *QueryHandlerTests.cs      # Query handler tests
```

## TestBase Class

All tests inherit from `TestBase` which provides:

### Mocked Dependencies
- `MockUnitOfWork` - Mocks the Unit of Work pattern
- `MockViewQueries` - Mocks database view queries (ISystemViewQueries)
- `MockDateTimeProvider` - Mocks time-dependent operations
- Individual repository mocks:
  - `MockCurrencyRepository`
  - `MockExchangeRateRepository`
  - `MockProviderRepository`
  - `MockUserRepository`
  - `MockFetchLogRepository`
  - `MockErrorLogRepository`

### Helper Methods
- `CreateMockLogger<T>()` - Creates logger mocks
- `VerifySaveChangesCalled()` - Verifies SaveChanges was called
- `VerifySaveChangesNotCalled()` - Verifies SaveChanges was NOT called

## Testing Commands

Commands are operations that **change state** (Create, Update, Delete).

### Example: CreateCurrencyCommand

```csharp
[Fact]
public async Task Handle_WithValidNewCurrency_ShouldCreateCurrencySuccessfully()
{
    // Arrange
    var command = new CreateCurrencyCommand("USD");

    // Mock: Currency doesn't exist
    MockCurrencyRepository
        .Setup(x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()))
        .ReturnsAsync((Currency?)null);

    // Mock: Add and SaveChanges succeed
    MockCurrencyRepository
        .Setup(x => x.AddAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

    MockUnitOfWork
        .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(1);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeTrue();

    // Verify interactions
    MockCurrencyRepository.Verify(
        x => x.AddAsync(It.Is<Currency>(c => c.Code == "USD"),
        It.IsAny<CancellationToken>()),
        Times.Once);

    VerifySaveChangesCalled();
}
```

### Test Scenarios for Commands
1. ✅ **Success Case** - Valid input, operation succeeds
2. ✅ **Validation Failures** - Invalid input (use Theory with InlineData)
3. ✅ **Business Rule Violations** - E.g., duplicate entity
4. ✅ **Repository Exceptions** - Database errors
5. ✅ **Cancellation Token Handling** - Token is passed through

## Testing Queries

Queries are operations that **read data** without side effects.

### Example: GetCurrentExchangeRatesQuery

```csharp
[Fact]
public async Task Handle_WithAvailableRates_ShouldReturnMappedDtos()
{
    // Arrange
    var query = new GetCurrentExchangeRatesQuery();

    var viewData = new List<CurrentExchangeRateView>
    {
        new()
        {
            Id = 1,
            ProviderCode = "ECB",
            BaseCurrencyCode = "EUR",
            TargetCurrencyCode = "USD",
            Rate = 1.0850m,
            Multiplier = 1,
            ValidDate = new DateOnly(2025, 11, 6),
            // ... other properties
        }
    };

    // Mock: View query returns data
    MockViewQueries
        .Setup(x => x.GetCurrentExchangeRatesAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(viewData);

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    result.Should().HaveCount(1);
    result.First().ProviderCode.Should().Be("ECB");
    result.First().Rate.Should().Be(1.0850m);

    // Verify view query was called
    MockViewQueries.Verify(
        x => x.GetCurrentExchangeRatesAsync(It.IsAny<CancellationToken>()),
        Times.Once);
}
```

### Test Scenarios for Queries
1. ✅ **With Data** - Returns mapped DTOs correctly
2. ✅ **Empty Results** - Returns empty list (not null)
3. ✅ **Data Transformation** - Calculations, mappings work correctly
4. ✅ **Filtering** - Query filters are applied
5. ✅ **Multiple Data Sources** - Queries from multiple views/repositories
6. ✅ **Cancellation Token Handling** - Token is passed through

## Best Practices

### 1. Arrange-Act-Assert Pattern
Always use clear AAA structure:
```csharp
// Arrange - Set up mocks and test data
var command = new CreateCurrencyCommand("USD");
MockRepository.Setup(...).Returns(...);

// Act - Execute the handler
var result = await _handler.Handle(command, CancellationToken.None);

// Assert - Verify the result
result.IsSuccess.Should().BeTrue();
MockRepository.Verify(..., Times.Once);
```

### 2. Use FluentAssertions
Prefer FluentAssertions for readable assertions:
```csharp
// ✅ Good - Readable
result.IsSuccess.Should().BeTrue();
result.Value.Should().BeGreaterThan(0);
result.Error.Should().Contain("already exists");

// ❌ Avoid - Less readable
Assert.True(result.IsSuccess);
Assert.True(result.Value > 0);
Assert.Contains("already exists", result.Error);
```

### 3. Test One Thing Per Test
Each test should verify a single behavior:
```csharp
// ✅ Good - Tests one scenario
[Fact]
public async Task Handle_WithExistingCurrency_ShouldReturnFailure()

// ❌ Avoid - Tests multiple scenarios
[Fact]
public async Task Handle_ShouldWorkCorrectly()
```

### 4. Use Theory for Similar Tests
Use `[Theory]` with `[InlineData]` for testing multiple similar cases:
```csharp
[Theory]
[InlineData("")]
[InlineData("  ")]
[InlineData("U")]
[InlineData("USDD")]
public async Task Handle_WithInvalidCode_ShouldFail(string invalidCode)
{
    // Test logic
}
```

### 5. Verify Mock Interactions
Always verify that mocks were called as expected:
```csharp
// Verify method was called once
MockRepository.Verify(x => x.AddAsync(...), Times.Once);

// Verify method was never called
MockRepository.Verify(x => x.DeleteAsync(...), Times.Never);

// Verify specific arguments
MockRepository.Verify(
    x => x.GetByCodeAsync(It.Is<string>(s => s == "USD"), ...),
    Times.Once);
```

### 6. Test Edge Cases
Always test boundary conditions:
- Empty strings
- Null values
- Zero/negative numbers
- Maximum values
- Empty collections

### 7. Name Tests Descriptively
Use clear naming: `MethodName_Scenario_ExpectedBehavior`
```csharp
Handle_WithValidNewCurrency_ShouldCreateCurrencySuccessfully
Handle_WithExistingCurrency_ShouldReturnFailure
Handle_WithInvalidCurrencyCode_ShouldReturnFailure
```

## Running Tests

### Run all tests:
```bash
cd TestLayer/Unit
dotnet test
```

### Run specific test:
```bash
dotnet test --filter "FullyQualifiedName~CreateCurrencyCommandHandlerTests"
```

### Run with coverage:
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Run with verbose output:
```bash
dotnet test --logger "console;verbosity=detailed"
```

## Dependencies

- **xUnit** - Test framework
- **Moq** - Mocking framework
- **FluentAssertions** - Assertion library
- **NSubstitute** - Alternative mocking framework

## Test Coverage Goals

Aim for:
- ✅ **90%+ Code Coverage** for ApplicationLayer handlers
- ✅ **100% Coverage** for critical business logic
- ✅ **All success paths** tested
- ✅ **All failure paths** tested
- ✅ **All edge cases** tested

## Adding New Tests

1. Create test class inheriting from `TestBase`
2. Set up handler in constructor
3. Write tests following AAA pattern
4. Use FluentAssertions for assertions
5. Verify mock interactions
6. Run tests and ensure they pass

## Example Test Class Template

```csharp
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Commands;

public class MyCommandHandlerTests : TestBase
{
    private readonly MyCommandHandler _handler;

    public MyCommandHandlerTests()
    {
        _handler = new MyCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<MyCommandHandler>().Object);
    }

    [Fact]
    public async Task Handle_Success_Scenario()
    {
        // Arrange
        var command = new MyCommand();

        MockRepository
            .Setup(x => x.MethodAsync(...))
            .ReturnsAsync(...);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        MockRepository.Verify(x => x.MethodAsync(...), Times.Once);
    }
}
```

## Additional Resources

- [xUnit Documentation](https://xunit.net/)
- [Moq Quickstart](https://github.com/moq/moq4/wiki/Quickstart)
- [FluentAssertions Documentation](https://fluentassertions.com/introduction)
- [Unit Testing Best Practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
