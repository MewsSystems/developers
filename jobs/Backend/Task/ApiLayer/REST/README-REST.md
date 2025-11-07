# REST Exchange Rates API

A comprehensive RESTful API for managing and querying exchange rates from multiple providers (CNB, ECB, BNR), built with ASP.NET Core following Clean Architecture principles.

## Overview

This REST API provides:
- **RESTful endpoints** for exchange rate queries and management
- **SignalR hubs** for real-time push notifications
- **JWT authentication** for secure access
- **Swagger/OpenAPI** documentation
- **Multi-provider support** (Czech National Bank, European Central Bank, Romanian National Bank)
- **Historical data** tracking and queries
- **Role-based access control** (Admin/Consumer)
- **Background job processing** with Hangfire

## Architecture

The REST API follows Clean Architecture with clear separation of concerns:

```
REST API (ApiLayer/REST)
    ↓
MediatR Commands/Queries (ApplicationLayer)
    ↓
Domain Logic (DomainLayer)
    ↓
Data Access (DataLayer, InfrastructureLayer)
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server or SQLite (in-memory for development)
- Modern web browser for Swagger UI

### Running the Server

```bash
cd ApiLayer/REST
dotnet run
```

The server will start on:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

### Swagger UI

Navigate to `https://localhost:5001/swagger` to explore the API interactively.

### Configuration

Configure the server via `appsettings.json`:

```json
{
  "Jwt": {
    "SecretKey": "your-secret-key-must-be-at-least-32-characters-long",
    "Issuer": "ExchangeRatesAPI",
    "Audience": "ExchangeRatesClient",
    "ExpirationMinutes": 60
  },
  "Database": {
    "UseInMemory": true,
    "ConnectionString": "Server=...;Database=ExchangeRates;..."
  },
  "Hangfire": {
    "DashboardPath": "/hangfire",
    "WorkerCount": 5
  }
}
```

## API Structure

### Areas

The API is organized into logical areas:

1. **Authentication** - `/api/authentication/*`
2. **Exchange Rates** - `/api/exchange-rates/*`
3. **Providers** - `/api/providers/*`
4. **Currencies** - `/api/currencies/*`
5. **Users** - `/api/users/*`
6. **System Health** - `/api/system-health/*`

## Authentication

### Login

**Endpoint:** `POST /api/authentication/login`

**Request:**
```json
{
  "email": "admin@example.com",
  "password": "simple"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2025-11-07T18:00:00Z",
  "user": {
    "id": 1,
    "email": "admin@example.com",
    "firstName": "Admin",
    "lastName": "User",
    "role": "Admin"
  }
}
```

### Register

**Endpoint:** `POST /api/authentication/register`

**Request:**
```json
{
  "email": "newuser@example.com",
  "password": "SecurePassword123!",
  "fullName": "John Doe",
  "role": "Consumer"
}
```

### Using Authentication

Include the JWT token in the Authorization header:

```bash
curl -H "Authorization: Bearer eyJhbGc..." \
  https://localhost:5001/api/currencies
```

## Exchange Rates Endpoints

### Get Latest Exchange Rates

**Endpoint:** `GET /api/exchange-rates/latest`

Returns the most recent exchange rates for all currency pairs.

**Response:**
```json
[
  {
    "baseCurrencyCode": "CZK",
    "targetCurrencyCode": "EUR",
    "rate": 0.0398,
    "amount": 1,
    "date": "2025-11-07",
    "providerCode": "CNB",
    "providerName": "Czech National Bank"
  },
  ...
]
```

### Get Latest Updated Rates

**Endpoint:** `GET /api/exchange-rates/latest-updated`

Returns only recently updated exchange rates.

### Get Historical Rates

**Endpoint:** `GET /api/exchange-rates/history/{baseCurrency}/{targetCurrency}`

**Parameters:**
- `baseCurrency` - Base currency code (e.g., "EUR")
- `targetCurrency` - Target currency code (e.g., "CZK")
- `startDate` (optional) - Start date (YYYY-MM-DD)
- `endDate` (optional) - End date (YYYY-MM-DD)
- `providerId` (optional) - Filter by provider

**Example:**
```bash
GET /api/exchange-rates/history/EUR/CZK?startDate=2025-10-01&endDate=2025-11-01
```

### Get Rates by Date

**Endpoint:** `GET /api/exchange-rates/by-date/{date}`

**Example:**
```bash
GET /api/exchange-rates/by-date/2025-11-07
```

### Get Rates by Date Range

**Endpoint:** `GET /api/exchange-rates/by-date-range`

**Query Parameters:**
- `startDate` - Start date (YYYY-MM-DD)
- `endDate` - End date (YYYY-MM-DD)
- `baseCurrency` (optional)
- `targetCurrency` (optional)

### Get Rates by Provider

**Endpoint:** `GET /api/exchange-rates/by-provider/{providerId}`

### Get Supported Currency Pairs

**Endpoint:** `GET /api/exchange-rates/currency-pairs`

Returns all available currency pair combinations.

### Convert Currency

**Endpoint:** `POST /api/exchange-rates/convert`

**Request:**
```json
{
  "fromCurrency": "EUR",
  "toCurrency": "CZK",
  "amount": 100,
  "date": "2025-11-07"
}
```

**Response:**
```json
{
  "fromCurrency": "EUR",
  "toCurrency": "CZK",
  "fromAmount": 100.0,
  "toAmount": 2513.50,
  "rate": 25.135,
  "date": "2025-11-07",
  "provider": "CNB"
}
```

## Providers Endpoints

### Get All Providers

**Endpoint:** `GET /api/providers`

**Response:**
```json
[
  {
    "id": 1,
    "code": "CNB",
    "name": "Czech National Bank",
    "url": "https://www.cnb.cz/...",
    "baseCurrencyCode": "CZK",
    "isActive": true,
    "status": "Healthy",
    "consecutiveFailures": 0,
    "lastSuccessfulFetch": "2025-11-07T16:00:00Z",
    "created": "2025-11-07T10:00:00Z"
  },
  ...
]
```

### Get Provider by ID

**Endpoint:** `GET /api/providers/{id}`

### Get Provider by Code

**Endpoint:** `GET /api/providers/by-code/{code}`

**Example:**
```bash
GET /api/providers/by-code/ECB
```

### Create Provider

**Endpoint:** `POST /api/providers` (Admin only)

**Request:**
```json
{
  "name": "New Bank",
  "code": "NBK",
  "url": "https://api.newbank.com/rates",
  "baseCurrencyCode": "USD",
  "requiresAuthentication": false
}
```

### Update Provider

**Endpoint:** `PUT /api/providers/{id}` (Admin only)

### Delete Provider

**Endpoint:** `DELETE /api/providers/{id}` (Admin only)

### Activate/Deactivate Provider

**Endpoints:**
- `POST /api/providers/{id}/activate` (Admin only)
- `POST /api/providers/{id}/deactivate` (Admin only)

### Get Provider Health

**Endpoint:** `GET /api/providers/{id}/health`

**Response:**
```json
{
  "providerId": 1,
  "providerCode": "CNB",
  "providerName": "Czech National Bank",
  "isActive": true,
  "status": "Healthy",
  "consecutiveFailures": 0,
  "lastSuccessfulFetch": "2025-11-07T16:00:00Z",
  "lastFailedFetch": null,
  "totalFetchAttempts": 150,
  "successfulFetches": 148,
  "failedFetches": 2,
  "totalRatesProvided": 14800,
  "successRate": 98.67,
  "firstFetchDate": "2025-08-01T10:00:00Z",
  "lastFetchDate": "2025-11-07T16:00:00Z"
}
```

### Get Provider Configuration

**Endpoint:** `GET /api/providers/{id}/configuration`

### Update Provider Configuration

**Endpoint:** `PUT /api/providers/{id}/configuration` (Admin only)

### Test Provider Connection

**Endpoint:** `POST /api/providers/{id}/test-connection` (Admin only)

## Currencies Endpoints

### Get All Currencies

**Endpoint:** `GET /api/currencies`

**Response:**
```json
[
  { "id": 1, "code": "CZK" },
  { "id": 2, "code": "EUR" },
  { "id": 3, "code": "RON" }
]
```

### Get Currency by ID

**Endpoint:** `GET /api/currencies/{id}`

### Get Currency by Code

**Endpoint:** `GET /api/currencies/by-code/{code}`

**Example:**
```bash
GET /api/currencies/by-code/EUR
```

### Create Currency

**Endpoint:** `POST /api/currencies` (Admin only)

**Request:**
```json
{
  "code": "USD"
}
```

### Delete Currency

**Endpoint:** `DELETE /api/currencies/{id}` (Admin only)

## Users Endpoints

### Get All Users

**Endpoint:** `GET /api/users` (Admin only)

**Query Parameters:**
- `pageNumber` (default: 1)
- `pageSize` (default: 10)

**Response:**
```json
{
  "items": [
    {
      "id": 1,
      "email": "admin@example.com",
      "firstName": "Admin",
      "lastName": "User",
      "role": "Admin"
    },
    ...
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 25,
  "totalPages": 3
}
```

### Get User by ID

**Endpoint:** `GET /api/users/{id}` (Admin or self)

### Get User by Email

**Endpoint:** `GET /api/users/by-email/{email}` (Admin or self)

### Create User

**Endpoint:** `POST /api/users` (Admin only)

**Request:**
```json
{
  "email": "newuser@example.com",
  "password": "SecurePassword123!",
  "fullName": "John Doe",
  "role": "Consumer"
}
```

### Update User Info

**Endpoint:** `PUT /api/users/{id}` (Admin or self)

**Request:**
```json
{
  "email": "updated@example.com",
  "fullName": "Jane Doe"
}
```

### Change Password

**Endpoint:** `PUT /api/users/{id}/password` (Admin or self)

**Request:**
```json
{
  "currentPassword": "OldPassword123!",
  "newPassword": "NewPassword123!"
}
```

### Delete User

**Endpoint:** `DELETE /api/users/{id}` (Admin only)

### Get Users by Role

**Endpoint:** `GET /api/users/by-role/{role}` (Admin only)

**Example:**
```bash
GET /api/users/by-role/Admin
```

### Update User Role

**Endpoint:** `PUT /api/users/{id}/role` (Admin only)

**Request:**
```json
{
  "role": "Admin"
}
```

### Get Current User

**Endpoint:** `GET /api/users/me`

Returns the authenticated user's profile.

## System Health Endpoints

### Get System Health

**Endpoint:** `GET /api/system-health`

**Response:**
```json
{
  "totalProviders": 3,
  "activeProviders": 3,
  "quarantinedProviders": 0,
  "totalCurrencies": 3,
  "totalExchangeRates": 15000,
  "latestRateDate": "2025-11-07",
  "oldestRateDate": "2023-11-07",
  "totalFetchesToday": 45,
  "successfulFetchesToday": 43,
  "failedFetchesToday": 2,
  "successRateToday": 95.56,
  "lastUpdated": "2025-11-07T17:00:00Z"
}
```

### Get Recent Errors

**Endpoint:** `GET /api/system-health/errors`

**Query Parameters:**
- `pageNumber` (default: 1)
- `pageSize` (default: 10)

**Response:**
```json
{
  "items": [
    {
      "id": 123,
      "severity": "Error",
      "source": "FetchLatestRatesJob",
      "message": "Failed to fetch rates from CNB",
      "occurredAt": "2025-11-07T15:30:00Z",
      "providerCode": "CNB",
      "providerName": "Czech National Bank"
    },
    ...
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 50,
  "totalPages": 5
}
```

### Get Fetch Activity

**Endpoint:** `GET /api/system-health/fetch-activity`

**Query Parameters:**
- `pageNumber` (default: 1)
- `pageSize` (default: 10)

**Response:**
```json
{
  "items": [
    {
      "id": 456,
      "fetchedAt": "2025-11-07T16:00:00Z",
      "isSuccess": true,
      "httpStatusCode": 200,
      "responseTimeMs": 345,
      "ratesFetched": 100,
      "providerCode": "ECB",
      "providerName": "European Central Bank"
    },
    ...
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 500,
  "totalPages": 50
}
```

## SignalR Real-Time Notifications

### Hub Endpoint

**URL:** `/hubs/exchange-rates`

Connect to receive real-time updates when exchange rates are fetched.

### JavaScript Client Example

```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/exchange-rates", {
        accessTokenFactory: () => jwtToken
    })
    .build();

// Listen for exchange rate updates
connection.on("ReceiveLatestRatesUpdated", (data) => {
    console.log("New rates received:", data);
    // data contains grouped exchange rates by provider
});

// Listen for provider status changes
connection.on("ReceiveProviderStatusChanged", (data) => {
    console.log("Provider status changed:", data);
    // data contains provider info and new status
});

// Start connection
await connection.start();

// Join exchange rates group to receive updates
await connection.invoke("JoinExchangeRatesGroup");
```

### .NET Client Example

```csharp
var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:5001/hubs/exchange-rates", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult(jwtToken);
    })
    .Build();

connection.On<ExchangeRatesGroupedData>("ReceiveLatestRatesUpdated", data =>
{
    Console.WriteLine($"Received {data.Providers.Count} provider updates");
});

connection.On<ProviderStatusData>("ReceiveProviderStatusChanged", data =>
{
    Console.WriteLine($"Provider {data.ProviderCode} is now {data.Status}");
});

await connection.StartAsync();
await connection.InvokeAsync("JoinExchangeRatesGroup");
```

### Available Events

- `ReceiveLatestRatesUpdated` - Triggered when new exchange rates are fetched
- `ReceiveProviderStatusChanged` - Triggered when a provider's status changes

## Background Jobs (Hangfire)

The API includes automated background jobs for fetching exchange rates.

### Hangfire Dashboard

Access the dashboard at `https://localhost:5001/hangfire`

**Default credentials:** (configured in appsettings)

### Scheduled Jobs

1. **Fetch Latest Rates** - Runs daily at 4 PM UTC (configurable via cron)
2. **Fetch Historical Rates** - Runs once on startup to backfill data
3. **Provider Health Checks** - Runs every 5 minutes

### Job Configuration

Configure in `appsettings.json`:

```json
{
  "BackgroundJobs": {
    "EnableOnStartup": true,
    "FetchLatestRatesCron": "0 16 * * *",
    "HealthCheckIntervalMinutes": 5,
    "HistoricalDataDays": 90
  }
}
```

## Default Users

Two users are seeded on startup:

| Email | Password | Role |
|-------|----------|------|
| `admin@example.com` | `simple` | Admin |
| `consumer@example.com` | `simple` | Consumer |

## Error Responses

All errors follow a consistent format:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "errors": {
    "Email": ["The Email field is required."]
  }
}
```

### Common Status Codes

- `200 OK` - Successful request
- `201 Created` - Resource created successfully
- `204 No Content` - Successful request with no response body
- `400 Bad Request` - Invalid request data
- `401 Unauthorized` - Missing or invalid JWT token
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `409 Conflict` - Resource conflict (e.g., duplicate email)
- `500 Internal Server Error` - Server error

## Rate Limiting

The API includes rate limiting to prevent abuse:

- **Authenticated users:** 100 requests per minute
- **Unauthenticated users:** 20 requests per minute

Configure in `appsettings.json`:

```json
{
  "RateLimiting": {
    "Enabled": true,
    "RequestsPerMinute": 100,
    "UnauthenticatedRequestsPerMinute": 20
  }
}
```

## CORS

CORS is configured to allow requests from specific origins:

```csharp
// Configure in Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://example.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
```

## Testing

### Swagger UI

Use the interactive Swagger UI at `/swagger` to test all endpoints.

### Postman

Import the OpenAPI definition from `/swagger/v1/swagger.json` into Postman.

### curl Examples

**Login:**
```bash
curl -X POST https://localhost:5001/api/authentication/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"simple"}'
```

**Get currencies (authenticated):**
```bash
curl https://localhost:5001/api/currencies \
  -H "Authorization: Bearer eyJhbGc..."
```

**Get latest rates:**
```bash
curl https://localhost:5001/api/exchange-rates/latest
```

## Known Issues

### SQLite Database Locking

When using in-memory SQLite, concurrent operations may cause locking errors:

```
SQLite Error 6: 'database table is locked'
```

**Solution:** Switch to SQL Server for production or wait for background jobs to complete.

## Production Deployment

### Database Migration

1. Update connection string in `appsettings.Production.json`
2. Run migrations:
   ```bash
   dotnet ef database update --project DataLayer
   ```

### HTTPS Certificate

Configure certificate in `appsettings.Production.json`:

```json
{
  "Kestrel": {
    "Endpoints": {
      "Https": {
        "Url": "https://*:443",
        "Certificate": {
          "Path": "/path/to/certificate.pfx",
          "Password": "certificate-password"
        }
      }
    }
  }
}
```

### Environment Variables

Override settings using environment variables:

```bash
export Jwt__SecretKey="production-secret-key-32-characters-minimum"
export Database__ConnectionString="Server=prod-db;Database=ExchangeRates;..."
```

### Docker

Build and run with Docker:

```bash
docker build -t exchange-rates-api .
docker run -p 5000:5000 -p 5001:5001 exchange-rates-api
```

## Performance Considerations

- **Caching:** Add response caching for frequently accessed endpoints
- **Database indexing:** Ensure proper indexes on `Date`, `ProviderId`, `CurrencyId`
- **Connection pooling:** Configure appropriate connection pool size
- **Compression:** Enable response compression for large payloads

## Monitoring

### Health Checks

Built-in health check endpoint:

```bash
curl https://localhost:5001/health
```

### Application Insights

Configure Azure Application Insights in `appsettings.json`:

```json
{
  "ApplicationInsights": {
    "ConnectionString": "InstrumentationKey=..."
  }
}
```

### Logging

Logs are written to:
- Console (development)
- File system (configurable)
- Application Insights (production)

Configure log levels in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

## API Versioning

The API supports versioning via URL path:

- v1: `/api/v1/currencies`
- v2: `/api/v2/currencies` (future)

## Further Reading

- [ASP.NET Core Web API](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
- [SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr/)
- [Hangfire](https://www.hangfire.io/)
- [Swagger/OpenAPI](https://swagger.io/specification/)

## Support

For issues or questions:
1. Check Swagger UI for endpoint documentation
2. Review server logs for detailed error messages
3. Test with Postman or curl to isolate client issues
4. Check Hangfire dashboard for background job status

## License

[Your License Here]
