# Rate Service Refactoring Guide

This README documents the approach to refactoring the `RateService` class using the Single Responsibility Principle (SRP).

## Current Structure

The current `RateService` class violates SRP by handling multiple responsibilities:
- Provider validation
- Rate fetching
- Currency validation
- Exchange rate calculation
- Currency conversion
- Error handling

## Proposed Refactoring

The proposed refactoring splits the `RateService` class into these components:

### Main Components

1. **RateService**: Main facade class that orchestrates the other services
   - Located in: `app/services/exchange_rate/rate_service.rb`

2. **RateServiceFactory**: Factory for creating RateService instances with all dependencies
   - Located in: `app/services/exchange_rate/components/rate_service_factory.rb`

3. **RateFetcherService**: Handles fetching rates from providers or cache
   - Located in: `app/services/exchange_rate/components/rate_fetcher_service.rb`

4. **CurrencyValidatorService**: Handles currency validation and availability checks
   - Located in: `app/services/exchange_rate/components/currency_validator_service.rb`

5. **RateCalculatorService**: Performs exchange rate calculations
   - Located in: `app/services/exchange_rate/components/rate_calculator_service.rb`

6. **CurrencyConverterService**: Handles currency conversion logic
   - Located in: `app/services/exchange_rate/components/currency_converter_service.rb`

### Supporting Components

7. **ProviderValidator**: Validates that providers implement the required interface
   - Located in: `app/services/exchange_rate/components/provider_validator.rb`

8. **ExchangeRateErrorHandler**: Centralized error handling for exchange rate operations
   - Located in: `app/services/exchange_rate/components/error_handler.rb`

9. **RateServiceCompatibility**: Module with compatibility methods for tests
   - Located in: `app/services/exchange_rate/components/rate_service_compatibility.rb`

## Benefits of the Refactoring

- **Improved Maintainability**: Each class has a single, well-defined responsibility
- **Better Testability**: Smaller, focused classes are easier to test
- **Enhanced Readability**: Each class name clearly conveys its purpose
- **Reduced Complexity**: Methods in each class are shorter and more focused
- **Better Encapsulation**: Implementation details are hidden behind well-defined interfaces

## Implementation Notes

The refactoring should be done incrementally to ensure that existing functionality continues to work. A recommended approach is:

1. Create each component class with its focused functionality
2. Update the main RateService to use these components
3. Add backward compatibility methods to ensure existing code works
4. Ensure tests continue to pass after each step

## Example Usage

```ruby
# Create a RateService using the factory
rate_service = RateService.create(provider, repository, cache_strategy)

# Get rates for specific currencies
rates = rate_service.get_rates(['USD', 'EUR'])

# Get a specific exchange rate
rate = rate_service.get_rate('USD', 'EUR')

# Convert an amount
result = rate_service.convert(100, 'USD', 'EUR')
```

## Future Improvements

Once this refactoring is complete, further improvements could include:
- Moving each class to its own file
- Adding more comprehensive tests for each component
- Implementing dependency injection for better testability
- Creating an interface/protocol for each component 