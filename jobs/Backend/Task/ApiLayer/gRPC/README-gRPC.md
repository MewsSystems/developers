# gRPC Exchange Rates API

A high-performance gRPC implementation of the Exchange Rates API, providing a binary protocol alternative to the REST API with full feature parity and real-time streaming capabilities.

## Overview

This gRPC API provides:
- **41 RPC methods** across 6 service domains
- **Server-side streaming** for real-time exchange rate updates (replacing SignalR)
- **Full feature parity** with the REST API
- **Shared business logic** via MediatR and Clean Architecture
- **JWT authentication** for secure access
- **HTTP/2** protocol support
- **Protocol Buffers** for efficient data serialization

## Architecture

The gRPC layer is a thin wrapper over existing application layers:

```
gRPC Services (ApiLayer/gRPC)
    ↓
MediatR Commands/Queries (ApplicationLayer)
    ↓
Domain Logic (DomainLayer)
    ↓
Data Access (DataLayer, InfrastructureLayer)
```

All business logic is shared between REST and gRPC APIs, ensuring consistency.

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- Windows, Linux, or macOS
- gRPC client tool (grpcurl, BloomRPC, or custom client)

### Running the Server

```bash
cd ApiLayer/gRPC
dotnet run
```

The server will start on `http://localhost:5001` (HTTP/2 only).

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
    "ConnectionString": "..."
  }
}
```

## Services

### 1. Authentication Service

**Package:** `exchangerates.authentication.v1`

| RPC | Description |
|-----|-------------|
| `Login` | Authenticate user and receive JWT token |
| `Register` | Register a new user account |

**Example:**
```bash
grpcurl -plaintext -d '{"email":"admin@example.com","password":"simple"}' \
  localhost:5001 exchangerates.authentication.v1.AuthenticationService/Login
```

### 2. Exchange Rates Service

**Package:** `exchangerates.exchangerates.v1`

| RPC | Description |
|-----|-------------|
| `GetAllLatestExchangeRates` | Get latest rates for all currencies |
| `GetAllLatestUpdatedExchangeRates` | Get recently updated rates |
| `GetExchangeRateHistory` | Get historical rates for a currency pair |
| `GetExchangeRatesByDate` | Get rates for a specific date |
| `GetExchangeRatesByDateRange` | Get rates within a date range |
| `GetExchangeRatesByProvider` | Get rates from a specific provider |
| `GetSupportedCurrencyPairs` | List all available currency pairs |
| `ConvertCurrency` | Convert amount between currencies |
| `StreamExchangeRateUpdates` | **Stream** real-time rate updates |

**Streaming Example:**
```csharp
var stream = client.StreamExchangeRateUpdates(new StreamSubscriptionRequest
{
    SubscriptionTypes = { "LatestRatesUpdated", "ProviderStatusChanged" }
});

await foreach (var update in stream.ResponseStream.ReadAllAsync())
{
    Console.WriteLine($"Update: {update.EventType}");
    // Handle update...
}
```

### 3. Providers Service

**Package:** `exchangerates.providers.v1`

| RPC | Description |
|-----|-------------|
| `GetAllProviders` | List all exchange rate providers |
| `GetProviderById` | Get provider details by ID |
| `GetProviderByCode` | Get provider details by code (CNB, ECB, BNR) |
| `CreateProvider` | Add a new provider |
| `UpdateProvider` | Update provider information |
| `DeleteProvider` | Remove a provider |
| `ActivateProvider` | Enable a provider |
| `DeactivateProvider` | Disable a provider |
| `GetProviderHealth` | Get provider health status |
| `GetProviderConfiguration` | Get provider settings |
| `UpdateProviderConfiguration` | Update provider settings |
| `TestProviderConnection` | Test provider connectivity |

### 4. Currencies Service

**Package:** `exchangerates.currencies.v1`

| RPC | Description |
|-----|-------------|
| `GetAllCurrencies` | List all supported currencies |
| `GetCurrencyById` | Get currency by ID |
| `GetCurrencyByCode` | Get currency by code (EUR, USD, etc.) |
| `CreateCurrency` | Add a new currency |
| `DeleteCurrency` | Remove a currency |

### 5. Users Service

**Package:** `exchangerates.users.v1`

| RPC | Description |
|-----|-------------|
| `GetAllUsers` | List all users (paginated) |
| `GetUserById` | Get user details by ID |
| `GetUserByEmail` | Get user details by email |
| `CreateUser` | Create a new user |
| `UpdateUserInfo` | Update user information |
| `ChangeUserPassword` | Change user password |
| `DeleteUser` | Remove a user |
| `GetUsersByRole` | Get users by role (Admin/Consumer) |
| `UpdateUserRole` | Change user's role |
| `GetCurrentUser` | Get authenticated user's details |

### 6. System Health Service

**Package:** `exchangerates.systemhealth.v1`

| RPC | Description |
|-----|-------------|
| `GetSystemHealth` | Get overall system health metrics |
| `GetRecentErrors` | Get recent error logs (paginated) |
| `GetFetchActivity` | Get recent fetch activity (paginated) |

## Authentication

All RPCs except `Login` and `Register` require JWT authentication.

### Obtaining a Token

```bash
# Login to get JWT token
grpcurl -plaintext -d '{"email":"admin@example.com","password":"simple"}' \
  localhost:5001 exchangerates.authentication.v1.AuthenticationService/Login
```

Response:
```json
{
  "success": true,
  "authenticationData": {
    "token": "eyJhbGc...",
    "expiresAt": "2025-11-07T17:30:00Z",
    "user": { ... }
  }
}
```

### Using the Token

Pass the token in metadata headers:

```bash
grpcurl -plaintext \
  -H "Authorization: Bearer eyJhbGc..." \
  localhost:5001 exchangerates.currencies.v1.CurrenciesService/GetAllCurrencies
```

In code:
```csharp
var headers = new Metadata
{
    { "Authorization", $"Bearer {token}" }
};

var response = await client.GetAllCurrenciesAsync(
    new GetAllCurrenciesRequest(),
    headers: headers
);
```

## Streaming

The gRPC API provides server-side streaming as a replacement for SignalR push notifications.

### Available Streams

#### Exchange Rate Updates

Subscribe to real-time exchange rate updates:

```csharp
var request = new StreamSubscriptionRequest();
request.SubscriptionTypes.Add("LatestRatesUpdated");
request.SubscriptionTypes.Add("ProviderStatusChanged");

using var stream = client.StreamExchangeRateUpdates(request);

await foreach (var update in stream.ResponseStream.ReadAllAsync(cancellationToken))
{
    switch (update.EventType)
    {
        case "LatestRatesUpdated":
            // Handle new exchange rates
            foreach (var providerData in update.LatestRatesUpdated.Providers)
            {
                Console.WriteLine($"Provider: {providerData.ProviderCode}");
                foreach (var currencyData in providerData.Currencies)
                {
                    Console.WriteLine($"  {currencyData.BaseCurrencyCode}");
                    foreach (var rate in currencyData.Rates)
                    {
                        Console.WriteLine($"    → {rate.TargetCurrencyCode}: {rate.Rate}");
                    }
                }
            }
            break;

        case "ProviderStatusChanged":
            // Handle provider status changes
            var statusData = update.ProviderStatusChanged;
            Console.WriteLine($"Provider {statusData.ProviderCode} is now {statusData.Status}");
            break;
    }
}
```

### Event Types

- `LatestRatesUpdated`: Broadcasted when new exchange rates are fetched
- `ProviderStatusChanged`: Broadcasted when a provider's status changes (active/inactive/quarantined)

## Testing

### Test Client

A test client is provided in `ApiLayer/gRPC.TestClient`:

```bash
cd ApiLayer/gRPC.TestClient
dotnet run
```

The test client demonstrates:
- Login authentication
- Fetching all currencies with JWT
- Getting system health

### Manual Testing with grpcurl

Install grpcurl:
```bash
# Windows (using Scoop)
scoop install grpcurl

# macOS
brew install grpcurl

# Linux
go install github.com/fullstorydev/grpcurl/cmd/grpcurl@latest
```

List available services:
```bash
grpcurl -plaintext localhost:5001 list
```

Describe a service:
```bash
grpcurl -plaintext localhost:5001 describe exchangerates.currencies.v1.CurrenciesService
```

### Testing with BloomRPC or Postman

Import the `.proto` files from the `Protos/` directory into your gRPC client tool.

## Project Structure

```
gRPC/
├── Protos/                          # Protocol Buffer definitions
│   ├── common.proto                 # Shared messages (Date, Error, Paging)
│   ├── authentication.proto         # Auth service
│   ├── exchange_rates.proto         # Exchange rates service + streaming
│   ├── providers.proto              # Providers management
│   ├── currencies.proto             # Currency management
│   ├── users.proto                  # User management
│   └── system_health.proto          # System monitoring
├── Services/                        # gRPC service implementations
│   ├── AuthenticationGrpcService.cs
│   ├── ExchangeRatesGrpcService.cs
│   ├── ProvidersGrpcService.cs
│   ├── CurrenciesGrpcService.cs
│   ├── UsersGrpcService.cs
│   └── SystemHealthGrpcService.cs
├── Mappers/                         # DTO ↔ Proto converters
│   ├── AuthenticationMappers.cs
│   ├── ExchangeRateMappers.cs
│   ├── ProviderMappers.cs
│   ├── CurrencyMappers.cs
│   ├── UserMappers.cs
│   ├── SystemHealthMappers.cs
│   └── CommonMappers.cs
├── Streaming/                       # Streaming infrastructure
│   ├── IExchangeRatesStreamManager.cs
│   ├── ExchangeRatesStreamManager.cs
│   └── ExchangeRatesGrpcNotificationService.cs
├── Program.cs                       # Server entry point
├── appsettings.json                 # Configuration
└── appsettings.Development.json     # Development overrides
```

## Default Users

Two users are seeded on startup:

| Email | Password | Role |
|-------|----------|------|
| `admin@example.com` | `simple` | Admin |
| `consumer@example.com` | `simple` | Consumer |

## Known Issues

### SQLite Database Locking

When using the in-memory SQLite database, concurrent operations may result in database locking errors:

```
SQLite Error 6: 'database table is locked'
```

**Cause:** Background jobs (Hangfire) fetching historical data while API requests try to access the database.

**Solutions:**
1. Wait for background jobs to complete (usually 30-60 seconds after startup)
2. Use SQL Server or PostgreSQL for production (configured in `appsettings.json`)
3. Disable background jobs in development:
   ```json
   {
     "BackgroundJobs": {
       "EnableOnStartup": false
     }
   }
   ```

This limitation affects both REST and gRPC APIs equally.

## Performance

gRPC offers several advantages over REST:

- **Binary protocol**: Faster serialization/deserialization
- **HTTP/2**: Multiplexing, header compression, server push
- **Streaming**: Efficient real-time updates without polling
- **Smaller payload size**: Protocol Buffers vs JSON

Typical improvements:
- 20-30% smaller payload sizes
- 15-25% faster response times
- Reduced latency for streaming vs SignalR/WebSocket

## Comparison with REST API

| Feature | REST API | gRPC API |
|---------|----------|----------|
| Protocol | HTTP/1.1 | HTTP/2 |
| Serialization | JSON | Protocol Buffers |
| Contract | OpenAPI/Swagger | .proto files |
| Real-time | SignalR (WebSocket) | Server Streaming |
| Authentication | JWT (Bearer token) | JWT (Metadata header) |
| Business Logic | Shared via MediatR | Shared via MediatR |
| Port | 5000/5001 | 5001 |
| Browser Support | Full | Limited (requires gRPC-Web) |

## Production Considerations

### Database

Switch from in-memory SQLite to a production database:

```json
{
  "Database": {
    "UseInMemory": false,
    "ConnectionString": "Server=...;Database=ExchangeRates;..."
  }
}
```

### HTTPS/TLS

Configure TLS certificates in `Program.cs`:

```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001, o => o.Protocols = HttpProtocols.Http2);
    options.ListenAnyIP(5443, o =>
    {
        o.Protocols = HttpProtocols.Http2;
        o.UseHttps("certificate.pfx", "password");
    });
});
```

### Load Balancing

gRPC supports client-side load balancing. Configure in client:

```csharp
var channel = GrpcChannel.ForAddress("dns:///api.example.com:443", new GrpcChannelOptions
{
    Credentials = ChannelCredentials.SecureSsl,
    ServiceConfig = new ServiceConfig
    {
        LoadBalancingConfigs = { new PickFirstConfig() }
    }
});
```

### Monitoring

Enable gRPC health checks:

```csharp
builder.Services.AddGrpcHealthChecks()
    .AddCheck("database", ...);

app.MapGrpcHealthChecksService();
```

## Troubleshooting

### "Connection refused"

Ensure the server is running on the correct port:
```bash
dotnet run --project ApiLayer/gRPC
```

### "Unimplemented"

The RPC method may not be implemented or the proto file version doesn't match the server.

### "Unauthenticated"

Ensure you're passing the JWT token in metadata:
```
metadata: { Authorization: "Bearer <token>" }
```

### "Internal server error"

Check server logs for details. Common causes:
- Database connectivity issues
- Invalid JWT configuration
- Background job conflicts (SQLite locking)

## Further Reading

- [gRPC Documentation](https://grpc.io/docs/)
- [Protocol Buffers Guide](https://protobuf.dev/)
- [ASP.NET Core gRPC](https://learn.microsoft.com/en-us/aspnet/core/grpc/)
- [gRPC vs REST](https://cloud.google.com/blog/products/api-management/understanding-grpc-openapi-and-rest)

## Support

For issues or questions:
1. Check server logs for detailed error messages
2. Verify proto definitions match server version
3. Test with grpcurl to isolate client issues
4. Review the test client code for examples

## License

[Your License Here]
