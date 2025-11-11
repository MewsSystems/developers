# Application Layer

## Overview

The Application Layer implements the application's business logic and use cases. It orchestrates the flow of data between the Domain Layer and external layers (Infrastructure, Presentation), implementing the CQRS (Command Query Responsibility Segregation) pattern using MediatR.

## Architecture Pattern

This layer follows **Clean Architecture** and **CQRS** principles:

- **Commands**: Operations that modify state (Create, Update, Delete)
- **Queries**: Operations that read data without side effects
- **Handlers**: Execute commands and queries
- **Behaviors**: Cross-cutting concerns via MediatR pipeline
- **DTOs**: Data Transfer Objects for API contracts

## Project Structure

```
ApplicationLayer/
├── Common/
│   ├── Abstractions/          # CQRS interfaces
│   │   ├── ICommand.cs         # Command marker interface
│   │   ├── ICommandHandler.cs  # Command handler interface
│   │   ├── IQuery.cs           # Query marker interface
│   │   └── IQueryHandler.cs    # Query handler interface
│   └── Exceptions/             # Application-level exceptions
│       ├── ApplicationException.cs
│       ├── ValidationException.cs
│       ├── NotFoundException.cs
│       ├── ConflictException.cs
│       └── ForbiddenException.cs
├── Behaviors/                  # MediatR pipeline behaviors
│   ├── UnhandledExceptionBehavior.cs
│   ├── LoggingBehavior.cs
│   ├── ValidationBehavior.cs
│   ├── PerformanceBehavior.cs
│   └── TransactionBehavior.cs
├── Commands/                   # Write operations
│   └── ExchangeRateProviders/
│       ├── CreateExchangeRateProvider/
│       │   ├── CreateExchangeRateProviderCommand.cs
│       │   ├── CreateExchangeRateProviderCommandHandler.cs
│       │   └── CreateExchangeRateProviderCommandValidator.cs
│       ├── ActivateProvider/
│       ├── DeactivateProvider/
│       └── ResetProviderHealth/
├── Queries/                    # Read operations
│   ├── Providers/
│   │   ├── GetProviderById/
│   │   │   ├── GetProviderByIdQuery.cs
│   │   │   ├── GetProviderByIdQueryHandler.cs
│   │   │   └── GetProviderByIdQueryValidator.cs
│   │   └── GetAllProviders/
│   └── ExchangeRates/
│       └── GetCurrentExchangeRates/
├── DTOs/                       # Data Transfer Objects
│   ├── Common/
│   │   ├── OperationResult.cs
│   │   └── PagedResult.cs
│   ├── ExchangeRates/
│   ├── ExchangeRateProviders/
│   ├── Currencies/
│   ├── Users/
│   └── SystemHealth/
└── ApplicationLayerServiceExtensions.cs
```

## CQRS Implementation

### Available Commands

The Application Layer implements the following commands organized by domain aggregate:

#### Currencies
- **CreateCurrency** - Creates a new currency in the system
  - Location: `Commands/Currencies/CreateCurrency/`
  - Input: Code, Name, Symbol
  - Output: `Result<int>` (Currency ID)

- **DeleteCurrency** - Deletes a currency from the system
  - Location: `Commands/Currencies/DeleteCurrency/`
  - Input: Currency ID
  - Output: `Result`

#### Exchange Rate Providers
- **CreateExchangeRateProvider** - Creates a new exchange rate provider
  - Location: `Commands/ExchangeRateProviders/CreateExchangeRateProvider/`
  - Input: Name, Code, URL, BaseCurrencyId, Authentication settings
  - Output: `Result<int>` (Provider ID)

- **ActivateProvider** - Activates a provider to resume fetching rates
  - Location: `Commands/ExchangeRateProviders/ActivateProvider/`
  - Input: Provider ID
  - Output: `Result`

- **DeactivateProvider** - Deactivates a provider to stop fetching rates
  - Location: `Commands/ExchangeRateProviders/DeactivateProvider/`
  - Input: Provider ID
  - Output: `Result`

- **DeleteProvider** - Deletes a provider from the system
  - Location: `Commands/ExchangeRateProviders/DeleteProvider/`
  - Input: Provider ID
  - Output: `Result`

- **ResetProviderHealth** - Resets provider health metrics (clears consecutive failures)
  - Location: `Commands/ExchangeRateProviders/ResetProviderHealth/`
  - Input: Provider ID
  - Output: `Result`

- **TriggerManualFetch** - Manually triggers a fetch for a specific provider
  - Location: `Commands/ExchangeRateProviders/TriggerManualFetch/`
  - Input: Provider ID
  - Output: `Result`

- **UpdateProviderConfiguration** - Updates provider configuration settings
  - Location: `Commands/ExchangeRateProviders/UpdateProviderConfiguration/`
  - Input: Provider ID, Configuration dictionary
  - Output: `Result`

#### Exchange Rates
- **BulkUpsertExchangeRates** - Bulk insert/update exchange rates for a provider
  - Location: `Commands/ExchangeRates/BulkUpsertExchangeRates/`
  - Input: Provider ID, ValidDate, List of rate items (SourceCurrencyCode, TargetCurrencyCode, Rate, Multiplier)
  - Output: `Result<BulkUpsertResultDto>` (RatesInserted, RatesUpdated, RatesUnchanged counts)

### Available Queries

The Application Layer implements the following queries organized by domain area:

#### Currencies
- **GetAllCurrencies** - Retrieves all currencies in the system
  - Location: `Queries/Currencies/GetAllCurrencies/`
  - Input: None
  - Output: `List<CurrencyDto>`

- **GetCurrencyByCode** - Retrieves a specific currency by its code
  - Location: `Queries/Currencies/GetCurrencyByCode/`
  - Input: Currency code (e.g., "USD")
  - Output: `CurrencyDto?`

- **GetCurrencyById** - Retrieves a specific currency by its ID
  - Location: `Queries/Currencies/GetCurrencyById/`
  - Input: Currency ID
  - Output: `CurrencyDto?`

#### Exchange Rates
- **ConvertCurrency** - Converts an amount between currencies using latest rates
  - Location: `Queries/ExchangeRates/ConvertCurrency/`
  - Input: Amount, SourceCurrencyCode, TargetCurrencyCode, Optional provider ID
  - Output: `CurrencyConversionDto` (ConvertedAmount, Rate, ValidDate)

- **GetCurrentExchangeRates** - Retrieves the most current exchange rates
  - Location: `Queries/ExchangeRates/GetCurrentExchangeRates/`
  - Input: Optional pagination parameters, optional filters
  - Output: `PagedResult<ExchangeRateDto>`

- **GetExchangeRateByProviderAndDate** - Retrieves rates for a specific provider and date
  - Location: `Queries/ExchangeRates/GetExchangeRateByProviderAndDate/`
  - Input: Provider ID, Date
  - Output: `List<ExchangeRateDto>`

- **GetExchangeRateHistory** - Retrieves historical rates for a currency pair
  - Location: `Queries/ExchangeRates/GetExchangeRateHistory/`
  - Input: SourceCurrencyCode, TargetCurrencyCode, StartDate, EndDate, Optional provider ID
  - Output: `List<ExchangeRateHistoryDto>`

- **GetLatestExchangeRate** - Retrieves the latest rate for a currency pair
  - Location: `Queries/ExchangeRates/GetLatestExchangeRate/`
  - Input: SourceCurrencyCode, TargetCurrencyCode, Optional provider ID
  - Output: `ExchangeRateDto?`

- **SearchExchangeRates** - Searches exchange rates with advanced filters
  - Location: `Queries/ExchangeRates/SearchExchangeRates/`
  - Input: SearchTerm, Filters, Pagination parameters
  - Output: `PagedResult<ExchangeRateDto>`

#### Providers
- **GetAllProviders** - Retrieves all exchange rate providers
  - Location: `Queries/Providers/GetAllProviders/`
  - Input: Optional filter (active only)
  - Output: `List<ExchangeRateProviderDto>`

- **GetProviderById** - Retrieves a specific provider with full details
  - Location: `Queries/Providers/GetProviderById/`
  - Input: Provider ID
  - Output: `ExchangeRateProviderDetailDto?` (includes configurations)

- **GetProviderConfiguration** - Retrieves provider configuration settings
  - Location: `Queries/Providers/GetProviderConfiguration/`
  - Input: Provider ID
  - Output: `Dictionary<string, string>` (Configuration key-value pairs)

- **GetProviderHealth** - Retrieves health metrics for a provider
  - Location: `Queries/Providers/GetProviderHealth/`
  - Input: Provider ID
  - Output: `ProviderHealthDto` (Status, ConsecutiveFailures, LastFetch, etc.)

- **GetProviderStatistics** - Retrieves statistics for a provider
  - Location: `Queries/Providers/GetProviderStatistics/`
  - Input: Provider ID, Optional date range
  - Output: `ProviderStatisticsDto` (TotalFetches, SuccessRate, AverageRatesPerFetch, etc.)

#### System Health
- **GetFetchActivity** - Retrieves recent fetch activity across all providers
  - Location: `Queries/SystemHealth/GetFetchActivity/`
  - Input: Optional date range, pagination parameters
  - Output: `PagedResult<FetchActivityDto>`

- **GetRecentErrors** - Retrieves recent errors from all providers
  - Location: `Queries/SystemHealth/GetRecentErrors/`
  - Input: Optional date range, pagination parameters
  - Output: `PagedResult<ProviderErrorDto>`

- **GetSystemHealth** - Retrieves overall system health status
  - Location: `Queries/SystemHealth/GetSystemHealth/`
  - Input: None
  - Output: `SystemHealthDto` (ActiveProviders, QuarantinedProviders, RecentErrors, LastSuccessfulFetch, etc.)

### Commands

Commands represent write operations that modify system state.

**Example: CreateExchangeRateProviderCommand**

```csharp
public record CreateExchangeRateProviderCommand(
    string Name,
    string Code,
    string Url,
    int BaseCurrencyId,
    bool RequiresAuthentication,
    string? ApiKeyVaultReference) : ICommand<Result<int>>;
```

**Handler:**

```csharp
public class CreateExchangeRateProviderCommandHandler
    : ICommandHandler<CreateExchangeRateProviderCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateExchangeRateProviderCommandHandler> _logger;

    public async Task<Result<int>> Handle(
        CreateExchangeRateProviderCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Validate business rules
        // 2. Create domain aggregate
        // 3. Persist changes
        // 4. Return result
    }
}
```

**Validator:**

```csharp
public class CreateExchangeRateProviderCommandValidator
    : AbstractValidator<CreateExchangeRateProviderCommand>
{
    public CreateExchangeRateProviderCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Provider name is required.")
            .MaximumLength(200);

        RuleFor(x => x.Code)
            .NotEmpty()
            .Matches("^[A-Z0-9_]+$");
    }
}
```

### Queries

Queries represent read operations that don't modify state.

**Example: GetProviderByIdQuery**

```csharp
public record GetProviderByIdQuery(int ProviderId)
    : IQuery<ExchangeRateProviderDetailDto?>;
```

**Handler:**

```csharp
public class GetProviderByIdQueryHandler
    : IQueryHandler<GetProviderByIdQuery, ExchangeRateProviderDetailDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public async Task<ExchangeRateProviderDetailDto?> Handle(
        GetProviderByIdQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Retrieve data from repository
        // 2. Map to DTO
        // 3. Return result
    }
}
```

## Pipeline Behaviors

MediatR pipeline behaviors implement cross-cutting concerns. They execute in the following order:

### 1. UnhandledExceptionBehavior (Outermost)

Catches and logs any unhandled exceptions.

```csharp
public class UnhandledExceptionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    // Logs errors and re-throws
}
```

### 2. LoggingBehavior

Logs request execution with timing information.

```csharp
public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    // Logs: "Handling {RequestName}"
    // Logs: "Handled {RequestName} in {ElapsedMs}ms"
}
```

### 3. ValidationBehavior

Validates requests using FluentValidation validators.

```csharp
public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    // Throws ValidationException if validation fails
}
```

### 4. PerformanceBehavior

Monitors performance and logs warnings for slow requests.

```csharp
public class PerformanceBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private const int PerformanceThresholdMs = 500;
    // Logs warning if request takes > 500ms
}
```

### 5. TransactionBehavior (Innermost)

Wraps commands in database transactions.

```csharp
public class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    // Only applies to ICommand<TResponse>
    // Begins transaction, commits on success, rolls back on failure
}
```

**Pipeline Flow:**

```
Request
  ↓
UnhandledExceptionBehavior
  ↓
LoggingBehavior
  ↓
ValidationBehavior (throws if invalid)
  ↓
PerformanceBehavior
  ↓
TransactionBehavior (commands only)
  ↓
Handler
  ↓
Response
```

## Data Transfer Objects (DTOs)

DTOs define contracts between layers and external systems.

### Common DTOs

**OperationResult:**

```csharp
public class OperationResult
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
}

public class OperationResult<T> : OperationResult
{
    public T? Data { get; set; }
}
```

**PagedResult:**

```csharp
public class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }
}
```

### Domain-Specific DTOs

**ExchangeRateProviderDto:**
```csharp
public class ExchangeRateProviderDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Status { get; set; }
    public int ConsecutiveFailures { get; set; }
    public DateTimeOffset? LastSuccessfulFetch { get; set; }
}
```

**ExchangeRateProviderDetailDto:**
```csharp
public class ExchangeRateProviderDetailDto : ExchangeRateProviderDto
{
    public List<ProviderConfigurationDto> Configurations { get; set; }
}
```

## Validation

Validation uses **FluentValidation** and executes in the `ValidationBehavior` pipeline.

### Creating Validators

```csharp
public class CreateExchangeRateProviderCommandValidator
    : AbstractValidator<CreateExchangeRateProviderCommand>
{
    public CreateExchangeRateProviderCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Provider name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Matches("^[A-Z0-9_]+$")
            .WithMessage("Code must contain only uppercase letters, numbers, and underscores.");

        RuleFor(x => x.Url)
            .NotEmpty()
            .Must(BeAValidUrl)
            .WithMessage("URL must be a valid HTTP or HTTPS URL.");

        RuleFor(x => x.ApiKeyVaultReference)
            .NotEmpty()
            .When(x => x.RequiresAuthentication)
            .WithMessage("API key vault reference is required when authentication is enabled.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
```

### Validation Errors

When validation fails, `ValidationException` is thrown:

```csharp
{
    "errors": {
        "Name": ["Provider name is required."],
        "Code": ["Code must contain only uppercase letters, numbers, and underscores."]
    }
}
```

## Exception Handling

### Application Exceptions

```csharp
ApplicationException (abstract base)
├── ValidationException      # Validation failures
├── NotFoundException       # Entity not found
├── ConflictException       # Business rule conflict
└── ForbiddenException      # Authorization failure
```

### Usage

```csharp
// In handlers
if (provider == null)
{
    throw new NotFoundException("Provider", request.ProviderId);
}

if (existingProvider != null)
{
    throw new ConflictException($"Provider with code '{request.Code}' already exists.");
}
```

### Domain Exceptions

Domain exceptions from the Domain Layer are caught and converted to appropriate application-level responses:

```csharp
try
{
    provider.Activate();
}
catch (ProviderQuarantinedException ex)
{
    return Result.Failure(ex.Message);
}
```

## Dependency Injection

### Registration

Add the Application Layer services in `Program.cs`:

```csharp
services.AddApplicationLayer();
```

This registers:
- MediatR with all handlers
- FluentValidation validators
- Pipeline behaviors
- CQRS abstractions

### Internal Registration

```csharp
public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
{
    var assembly = Assembly.GetExecutingAssembly();

    // MediatR (CQRS)
    services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(assembly);

        // Pipeline behaviors (order matters!)
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
    });

    // FluentValidation
    services.AddValidatorsFromAssembly(assembly);

    return services;
}
```

## Usage Examples

### In Controllers/Endpoints

```csharp
public class ProvidersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProvidersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProvider(
        CreateExchangeRateProviderCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(GetProvider),
                new { id = result.Value },
                result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProvider(int id)
    {
        var query = new GetProviderByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("{id}/activate")]
    public async Task<IActionResult> ActivateProvider(int id)
    {
        var command = new ActivateProviderCommand(id);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }
}
```

### In Services/Background Jobs

```csharp
public class ExchangeRateFetcherService
{
    private readonly IMediator _mediator;

    public async Task FetchRatesAsync()
    {
        // Query active providers
        var query = new GetActiveProvidersQuery();
        var providers = await _mediator.Send(query);

        foreach (var provider in providers)
        {
            // Execute command for each provider
            var command = new FetchExchangeRatesCommand(provider.Id);
            await _mediator.Send(command);
        }
    }
}
```

## Best Practices

### 1. Command/Query Separation

```csharp
// ✅ Good: Separate command and query
public record GetProviderByIdQuery(int Id) : IQuery<ProviderDto>;
public record UpdateProviderCommand(int Id, string Name) : ICommand<Result>;

// ❌ Bad: Mixing read and write
public record UpdateAndGetProviderCommand(int Id) : ICommand<ProviderDto>;
```

### 2. Validation in Application Layer

```csharp
// ✅ Good: Validate in Application Layer
public class CreateProviderValidator : AbstractValidator<CreateProviderCommand>
{
    RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
}

// ❌ Bad: Validation in controller
if (string.IsNullOrEmpty(command.Code)) return BadRequest();
```

### 3. Use Domain Exceptions

```csharp
// ✅ Good: Let domain throw exceptions, catch in handler
try
{
    provider.Activate();
}
catch (ProviderQuarantinedException ex)
{
    return Result.Failure(ex.Message);
}

// ❌ Bad: Check before calling domain method
if (provider.IsQuarantined)
    return Result.Failure("Quarantined");
```

### 4. DTOs Over Domain Entities

```csharp
// ✅ Good: Return DTO
public record GetProviderByIdQuery(int Id) : IQuery<ProviderDto>;

// ❌ Bad: Return domain entity
public record GetProviderByIdQuery(int Id) : IQuery<ExchangeRateProvider>;
```

### 5. Handler Organization

Organize handlers by feature/aggregate:

```
Commands/
└── ExchangeRateProviders/
    ├── CreateExchangeRateProvider/
    │   ├── Command.cs
    │   ├── Handler.cs
    │   └── Validator.cs
    └── UpdateExchangeRateProvider/
        ├── Command.cs
        ├── Handler.cs
        └── Validator.cs
```

## Dependencies

```xml
<ItemGroup>
  <!-- Domain Layer -->
  <ProjectReference Include="..\DomainLayer\DomainLayer.csproj" />

  <!-- MediatR (CQRS) -->
  <PackageReference Include="MediatR" />

  <!-- FluentValidation -->
  <PackageReference Include="FluentValidation.DependencyInjectionExtensions" />

  <!-- Logging -->
  <PackageReference Include="Microsoft.Extensions.Logging.Abstractions" />
</ItemGroup>
```

## Testing

### Unit Testing Commands

```csharp
public class CreateExchangeRateProviderCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var unitOfWork = new Mock<IUnitOfWork>();
        var logger = new Mock<ILogger<CreateExchangeRateProviderCommandHandler>>();
        var handler = new CreateExchangeRateProviderCommandHandler(
            unitOfWork.Object,
            logger.Object);

        var command = new CreateExchangeRateProviderCommand(
            "Test Provider", "TEST", "https://test.com", 1, false, null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
```

### Testing Validators

```csharp
public class CreateExchangeRateProviderCommandValidatorTests
{
    private readonly CreateExchangeRateProviderCommandValidator _validator;

    public CreateExchangeRateProviderCommandValidatorTests()
    {
        _validator = new CreateExchangeRateProviderCommandValidator();
    }

    [Fact]
    public void Validate_EmptyName_ReturnsValidationError()
    {
        // Arrange
        var command = new CreateExchangeRateProviderCommand(
            "", "TEST", "https://test.com", 1, false, null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.Name));
    }
}
```

## Related Documentation

- [Domain Layer README](../DomainLayer/README-DomainLayer.md)
- [Clean Architecture Principles](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
