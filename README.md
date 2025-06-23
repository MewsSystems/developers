# Exchange Rate Service

This is a Ruby on Rails application that provides exchange rates from the Czech National Bank (CNB). It follows clean architecture principles with a clear separation of concerns between domain, service, repository, provider, and controller layers.

## Architecture

The application is structured with the following layers:

- **Domain Layer**: Contains the core domain models (`Currency`, `ExchangeRate`) that are independent of any external data source.
- **Provider Layer**: Implements the strategy pattern to fetch exchange rate data from external sources (currently CNB and ECB).
  - Exposes metadata including supported currencies
  - Provides method to check currency support
- **Adapter Layer**: Handles the parsing of data from external sources into domain objects.
  - Uses format-specific adapters (Text, XML, JSON) selected through an adapter factory
  - Supports automatic format detection and content-type based selection
- **Repository Layer**: Provides an abstraction for data storage, allowing for multiple implementations (Redis-based with persistence).
  - Supports time-based expiration of cached rates
  - Gracefully handles Redis connection issues
- **Service Layer**: Coordinates the fetching and caching of exchange rate data, implements business logic.
  - Tracks unavailable currency requests
  - Provides warnings without failing the request
- **Controller Layer**: Exposes a REST API for clients to consume exchange rate data.
  - Includes warning section for unavailable currencies
- **DTO Layer**: Transforms domain objects into API response format, keeping serialization separate from domain logic.

## Key Features

- Fetch exchange rates from the Czech National Bank (CNB) and European Central Bank (ECB)
- Filter exchange rates by currency codes
- Convert amounts between different currencies
- Time-aware caching that respects provider publication schedules
- Provider metadata for data consistency validation
- Multi-format data support (TXT, XML, JSON) with automatic detection
- Robust encoding handling for international character sets
- Currency support validation with clear warning mechanism
- Clean design that allows for easy swapping of data sources and storage mechanisms

## API

The application exposes the following endpoints:

### Versioned API (Recommended)

```
GET /api/v1/exchange_rates
GET /api/v1/exchange_rates/:from/:to
GET /api/v1/exchange_rates/convert
GET /api/v1/exchange_rates/currencies
```

### Legacy Routes (Backward Compatibility)

```
GET /exchange_rates
GET /exchange_rates/convert
GET /exchange_rates/supported_currencies
```

### Health Check

```
GET /health
```

### Exchange Rates Endpoint

```
GET /api/v1/exchange_rates
```

Optional query parameter:
- `currencies`: A comma-separated list of currency codes to filter the results.

Example: `/api/v1/exchange_rates?currencies=USD,EUR,JPY`

Response format:
```json
{
  "base_currency": "CZK",
  "source": "Czech National Bank (CNB)",
  "timestamp": "2023-03-30T15:30:45Z",
  "rates": [
    {
      "from": "CZK",
      "to": "USD",
      "rate": 23.117,
      "date": "2023-03-30"
    },
    {
      "from": "CZK",
      "to": "EUR",
      "rate": 24.93,
      "date": "2023-03-30"
    }
  ],
  "warnings": {
    "unavailable_currencies": ["GBP"],
    "available_currencies": ["USD", "EUR", "JPY", "PLN", "HUF", ...],
    "provider": "Czech National Bank (CNB)"
  }
}
```

The `warnings` section only appears if requested currencies are not available from the current provider.

### Specific Rate Endpoint

```
GET /api/v1/exchange_rates/:from/:to
```

Example: `/api/v1/exchange_rates/CZK/USD`

Response format:
```json
{
  "from": "CZK",
  "to": "USD",
  "rate": 23.117,
  "date": "2023-03-30",
  "timestamp": "2023-03-30T15:30:45Z",
  "source": "Czech National Bank (CNB)"
}
```

### Currency Conversion Endpoint

```
GET /api/v1/exchange_rates/convert
```

Required query parameters:
- `from`: Source currency code
- `to`: Target currency code
- `amount`: Amount to convert

Example: `/api/v1/exchange_rates/convert?from=CZK&to=USD&amount=1000`

Response format:
```json
{
  "from": "CZK",
  "to": "USD",
  "amount": 1000,
  "converted_amount": 43.26,
  "rate": 0.04326,
  "timestamp": "2023-03-30T15:30:45Z",
  "source": "Czech National Bank (CNB)"
}
```

### Supported Currencies Endpoint

```
GET /api/v1/exchange_rates/currencies
```

Response format:
```json
{
  "source": "Czech National Bank (CNB)",
  "base_currency": "CZK",
  "currencies": [
    { "code": "USD" },
    { "code": "EUR" },
    { "code": "JPY" },
    // ...
  ]
}
```

### Health Check Endpoint

```
GET /health
```

Response format:
```json
{
  "status": "ok",
  "version": "1.0.0",
  "timestamp": "2023-03-30T15:30:45Z",
  "services": {
    "redis": {
      "status": "ok",
      "latency_ms": 2
    },
    "provider": {
      "status": "ok",
      "name": "Czech National Bank (CNB)",
      "latency_ms": 150
    }
  }
}
```

## Setup

1. Clone the repository
2. Install dependencies: `bundle install`
3. Start the server: `rails server`

### Docker Setup

The application includes Docker support for easy deployment:

```bash
docker-compose up
```

### Quick Start with Docker

To quickly test the application:

1. Navigate to the project directory and start the application:
   ```bash
   cd project_dir
   docker-compose up
   ```

2. In another terminal, test the API using curl:
   
   Get all exchange rates:
   ```bash
   curl http://localhost:3000/exchange_rates | jq
   ```
   
   Filter exchange rates by specific currencies:
   ```bash
   curl "http://localhost:3000/exchange_rates?currencies=USD,EUR,GBP" | jq
   ```

Note: The `jq` command formats the JSON response. If you don't have it installed, you can omit the `| jq` part.

### Redis Configuration

The application uses Redis for caching exchange rates with persistence. You can configure Redis through environment variables:

- `REDIS_HOST`: Redis server hostname (default: localhost)
- `REDIS_PORT`: Redis server port (default: 6379)
- `REDIS_DB`: Redis database number (default: 0)
- `REDIS_PASSWORD`: Redis password (if authentication required)
- `REDIS_URL`: Full Redis URL (overrides individual settings)

For production deployments, it's recommended to set these variables in your environment or container configuration.

Example:
```
REDIS_URL=redis://username:password@redis.example.com:6379/0 rails server
```

If Redis is unavailable, the application will log a warning but continue to function (exchange rates will not be persisted between application restarts).

## Testing

Run the test suite with:

```
bundle exec rspec
```

This will run all unit and integration tests.

## Testing with Different Providers

The application is designed to work with multiple exchange rate providers. Tests can be run against any of these providers to ensure compatibility.

### Available Providers

- **CNB**: Czech National Bank 
- **ECB**: European Central Bank
- **Mock**: A simulated provider for testing

### Running Tests with a Specific Provider

You can specify which provider to use for testing by setting the `EXCHANGE_RATE_TEST_PROVIDER` environment variable:

```bash
# Test with CNB
EXCHANGE_RATE_TEST_PROVIDER=cnb bundle exec rspec

# Test with ECB 
EXCHANGE_RATE_TEST_PROVIDER=ecb bundle exec rspec

# Test with mock provider (default)
EXCHANGE_RATE_TEST_PROVIDER=mock bundle exec rspec
```

### Provider-Specific Configuration

You can customize the provider configuration by setting additional environment variables:

```bash
# Custom CNB URL
CNB_TEST_URL=https://example.com/rates.txt EXCHANGE_RATE_TEST_PROVIDER=cnb bundle exec rspec

# Custom ECB URL
ECB_TEST_URL=https://example.com/rates.xml EXCHANGE_RATE_TEST_PROVIDER=ecb bundle exec rspec

# Custom mock provider base currency
MOCK_BASE_CURRENCY=EUR EXCHANGE_RATE_TEST_PROVIDER=mock bundle exec rspec
```

### Continuous Integration

The CI workflow runs tests with all providers in parallel to ensure the system works correctly with each provider.

## Design Considerations

The application is designed to be easily extensible:

- **Adding a new provider**: Implement the `BaseProvider` interface and add your new provider's configuration to `settings.yml`.
- **Changing the storage**: Implement a new repository that inherits from `ExchangeRateRepository`.
- **Supporting more formats**: Add a new adapter strategy that inherits from `BaseAdapter`.
- **Supporting more currencies**: The architecture already handles currencies with different amount factors from the CNB feed.

## Data Consistency & Provider Metadata

The application includes a provider metadata system that captures important details about each exchange rate source:

- **Update frequency**: How often rates are updated (daily, hourly, real-time)
- **Publication time**: When new rates are typically published each day
- **Working day handling**: Whether rates are only published on business days
- **Base currency**: The reference currency for all rates
- **Supported currencies**: Complete list of currencies available from this provider

This metadata is used by the service layer to:

1. Determine the appropriate date to fetch rates for (e.g., if current time is before today's publication, use previous business day's rates)
2. Handle weekend/holiday scenarios correctly when working days only is specified
3. Validate requested currencies against what's available from the provider
4. Provide transparency about data source capabilities

This ensures the application always serves the most appropriate rates based on the current time and provider publication schedule.

## Currency Coverage Validation

The application handles currency coverage differences between providers by:

- **Tracking and exposing provider-supported currencies**: Each provider's metadata includes the complete list of supported currencies.
- **Supporting currency validation**: The `supports_currency?` method allows checking if a currency is supported without fetching rates.
- **Tracking unavailable currency requests**: The service tracks which unavailable currencies have been requested.
- **Providing helpful feedback**: When unavailable currencies are requested, the API response includes a `warnings` section with:
  - List of requested but unavailable currencies
  - Provider name
  - Complete list of available currencies

This approach ensures:
1. Users get data for the currencies that are available
2. Users are informed when certain currencies are unavailable
3. Requests still succeed even with partial availability
4. Different providers with different currency coverage can be swapped in without breaking clients

## Multi-Format Support

The application supports multiple data formats through a flexible adapter system:

- **Format-Specific Adapters**: Dedicated adapters for TXT, XML, and JSON formats
- **Automatic Format Detection**: Can detect data format from:
  - Content-Type headers
  - File extensions
  - Content analysis (XML tags, JSON structure)
- **Encoding Handling**: Properly handles different character encodings (e.g., ISO-8859-2 used by CNB)
- **Consistent Domain Model**: All adapters convert to the same domain model regardless of source format

This flexibility allows:
1. Switching between different CNB endpoints (text, XML, JSON API) with minimal configuration
2. Adding support for new providers with different data formats
3. Migrating to new API versions without changing the rest of the application

The format is configurable in `settings.yml` or can be auto-detected from the response. 

## Adding a New Provider

To add a new exchange rate provider:

1. Create a new provider class that extends `BaseProvider`
2. Implement the required interface methods (`fetch_rates`, `metadata`)
3. Register the provider in `ProviderFactory`
4. Add provider-specific tests
5. Update the CI configuration to include the new provider

See the [Provider Template Documentation](docs/templates/provider_template.md) for a detailed guide and example implementation.

## Development

### Setup

```bash
bundle install
```

### Running Tests

```bash
bundle exec rspec
```

### Running Tests with Coverage

```bash
COVERAGE=true bundle exec rspec
```

### Code Style with RuboCop

This project uses RuboCop to enforce consistent code style across the codebase. RuboCop configuration is specified in `.rubocop.yml`.

To run RuboCop:

```bash
bundle exec rubocop
```

To automatically fix simple issues:

```bash
bundle exec rubocop -a
```

For safe auto-corrections only:

```bash
bundle exec rubocop --safe-auto-correct
```

To see only a specific cop's offenses:

```bash
bundle exec rubocop --only Style/StringLiterals
``` 