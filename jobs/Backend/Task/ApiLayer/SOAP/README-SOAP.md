# SOAP Exchange Rates API

A SOAP 1.1 implementation of the Exchange Rates API, providing traditional XML-based web services with WSDL support and full feature parity with the REST API.

## Overview

This SOAP API provides:
- **13 SOAP operations** across 5 service domains
- **WSDL-based service contracts** for automatic client generation
- **SignalR hub** for real-time exchange rate updates
- **Full feature parity** with core REST API functionality
- **Shared business logic** via MediatR and Clean Architecture
- **JWT authentication** for secure access
- **DataContractSerializer** for XML serialization
- **SOAP 1.1** protocol support

## Architecture

The SOAP layer is a thin wrapper over existing application layers:

```
SOAP Services (ApiLayer/SOAP)
    ↓
MediatR Commands/Queries (ApplicationLayer)
    ↓
Domain Logic (DomainLayer)
    ↓
Data Access (DataLayer, InfrastructureLayer)
```

All business logic is shared between REST, gRPC, and SOAP APIs, ensuring consistency across all protocols.

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- Windows, Linux, or macOS
- SOAP client tool (SoapUI, Postman, or .NET client)

### Running the Server

```bash
cd ApiLayer/SOAP
dotnet run
```

The server will start on:
- HTTP: `http://localhost:5002`
- HTTPS: `https://localhost:7002`

### Configuration

Configure the server via `appsettings.json`:

```json
{
  "Authentication": {
    "Jwt": {
      "SecretKey": "SuperSecretKey",
      "Issuer": "ExchangeRateApi",
      "Audience": "ExchangeRateApiClients",
      "ExpirationMinutes": 60
    }
  },
  "Database": {
    "UseInMemoryDatabase": false
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=ExchangeRateUpdaterTestSOAP;..."
  }
}
```

## Services

### 1. Authentication Service

**WSDL:** `http://localhost:5002/AuthenticationService.asmx?wsdl`

| Operation | Description |
|-----------|-------------|
| `Login` | Authenticate user and receive JWT token |

**Example Request:**
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <LoginRequest>
      <Email>admin@example.com</Email>
      <Password>simple</Password>
    </LoginRequest>
  </soap:Body>
</soap:Envelope>
```

**Example Response:**
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <LoginResponse>
      <Success>true</Success>
      <Message>Login successful</Message>
      <Data>
        <UserId>1</UserId>
        <Email>admin@example.com</Email>
        <FirstName>Admin</FirstName>
        <LastName>User</LastName>
        <Role>Admin</Role>
        <AccessToken>eyJhbGc...</AccessToken>
        <RefreshToken>refresh_token_here</RefreshToken>
        <ExpiresAt>1699999999</ExpiresAt>
      </Data>
    </LoginResponse>
  </soap:Body>
</soap:Envelope>
```

### 2. Exchange Rates Service

**WSDL:** `http://localhost:5002/ExchangeRateService.asmx?wsdl`

| Operation | Description |
|-----------|-------------|
| `GetAllLatestRatesGrouped` | Get latest rates grouped by provider and currency |
| `GetHistoricalRatesUpdate` | Get historical rates update notification data |

**Example Request:**
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <GetAllLatestRatesGroupedRequest />
  </soap:Body>
</soap:Envelope>
```

**Example Response:**
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <GetAllLatestRatesGroupedResponse>
      <Success>true</Success>
      <Message>Success</Message>
      <Data>
        <LatestExchangeRatesGroupedSoap>
          <ProviderCode>ECB</ProviderCode>
          <ProviderName>European Central Bank</ProviderName>
          <BaseCurrencyCode>EUR</BaseCurrencyCode>
          <Rates>
            <RateInfoSoap>
              <TargetCurrencyCode>USD</TargetCurrencyCode>
              <Rate>1.0850</Rate>
              <LastUpdated>2025-11-07T16:00:00Z</LastUpdated>
            </RateInfoSoap>
            <RateInfoSoap>
              <TargetCurrencyCode>CZK</TargetCurrencyCode>
              <Rate>25.340</Rate>
              <LastUpdated>2025-11-07T16:00:00Z</LastUpdated>
            </RateInfoSoap>
          </Rates>
        </LatestExchangeRatesGroupedSoap>
      </Data>
    </GetAllLatestRatesGroupedResponse>
  </soap:Body>
</soap:Envelope>
```

### 3. Currencies Service

**WSDL:** `http://localhost:5002/CurrencyService.asmx?wsdl`

| Operation | Description |
|-----------|-------------|
| `GetAllCurrencies` | List all supported currencies |
| `GetCurrencyByCode` | Get currency by code (EUR, USD, CZK, etc.) |

**Example Request:**
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <GetCurrencyByCodeRequest>
      <Code>EUR</Code>
    </GetCurrencyByCodeRequest>
  </soap:Body>
</soap:Envelope>
```

### 4. Providers Service

**WSDL:** `http://localhost:5002/ProviderService.asmx?wsdl`

| Operation | Description |
|-----------|-------------|
| `GetAllProviders` | List all exchange rate providers |
| `GetProviderById` | Get provider details by ID |

### 5. Users Service

**WSDL:** `http://localhost:5002/UserService.asmx?wsdl`

| Operation | Description |
|-----------|-------------|
| `GetAllUsers` | List all users (Admin only) |
| `GetUserById` | Get user details by ID |
| `GetUserByEmail` | Get user details by email |
| `CreateUser` | Create a new user (Admin only) |
| `DeleteUser` | Remove a user (Admin only) |
| `ChangeUserRole` | Change user's role (Admin only) |

## Real-Time Updates (SignalR)

**Hub URL:** `http://localhost:5002/hubs/exchange-rates`

The SOAP API includes a SignalR hub for real-time exchange rate notifications, enabling clients to receive push updates when new rates are fetched.

### Events

| Event | Description | Payload |
|-------|-------------|---------|
| `LatestRatesUpdated` | Triggered when daily exchange rates are fetched | XML string (DataContract serialized) |
| `HistoricalRatesUpdated` | Triggered when historical rates are fetched | XML string (DataContract serialized) |
| `Connected` | Triggered when client connects | Connection info (JSON) |

### Connecting to the Hub

**JavaScript Example:**
```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5002/hubs/exchange-rates", {
        accessTokenFactory: () => "your-jwt-token-here"
    })
    .withAutomaticReconnect()
    .build();

// Listen for latest rates updates (receives SOAP envelope as XML string)
connection.on("LatestRatesUpdated", (xmlString) => {
    console.log("Latest rates updated (SOAP XML):", xmlString);

    // Parse SOAP envelope
    const parser = new DOMParser();
    const xmlDoc = parser.parseFromString(xmlString, "text/xml");

    // Navigate to SOAP Body
    const body = xmlDoc.getElementsByTagNameNS("http://schemas.xmlsoap.org/soap/envelope/", "Body")[0];

    // Extract data from Body
    const providers = body.getElementsByTagName("LatestExchangeRatesGroupedSoap");
    console.log(`Received ${providers.length} provider groups`);

    // Process each provider...
    for (let i = 0; i < providers.length; i++) {
        const providerCode = providers[i].getElementsByTagName("Code")[0].textContent;
        console.log(`Provider: ${providerCode}`);
    }
});

// Listen for historical rates updates (receives SOAP envelope as XML string)
connection.on("HistoricalRatesUpdated", (xmlString) => {
    console.log("Historical rates updated (SOAP XML):", xmlString);

    // Parse SOAP envelope
    const parser = new DOMParser();
    const xmlDoc = parser.parseFromString(xmlString, "text/xml");

    // Navigate to SOAP Body
    const body = xmlDoc.getElementsByTagNameNS("http://schemas.xmlsoap.org/soap/envelope/", "Body")[0];

    // Extract and process data...
});

// Listen for connection confirmation
connection.on("Connected", (data) => {
    console.log("Connected to hub:", data);
});

await connection.start();
console.log("SignalR Connected");
```

**C# Example:**
```csharp
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using SOAP.Models.ExchangeRates;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5002/hubs/exchange-rates", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult("your-jwt-token-here");
    })
    .WithAutomaticReconnect()
    .Build();

// Helper method to deserialize SOAP envelope XML string
LatestExchangeRatesGroupedSoap[] DeserializeSoapXml(string soapXml)
{
    // Parse the SOAP envelope
    var doc = XDocument.Parse(soapXml);
    var soapNamespace = XNamespace.Get("http://schemas.xmlsoap.org/soap/envelope/");

    // Navigate to SOAP Body element
    var bodyElement = doc.Root?.Element(soapNamespace + "Body");
    if (bodyElement == null)
        throw new InvalidOperationException("SOAP Body element not found");

    // Get the first child element inside Body (the actual data)
    var dataElement = bodyElement.FirstNode;
    if (dataElement == null)
        throw new InvalidOperationException("No data found inside SOAP Body");

    // Deserialize the data inside the Body
    var serializer = new DataContractSerializer(typeof(LatestExchangeRatesGroupedSoap[]));
    using var reader = dataElement.CreateReader();
    return (LatestExchangeRatesGroupedSoap[])serializer.ReadObject(reader)!;
}

// Listen for latest rates (receives SOAP envelope as XML string)
connection.On<string>("LatestRatesUpdated", (soapXml) =>
{
    Console.WriteLine("Latest rates updated (SOAP XML)");
    var rates = DeserializeSoapXml(soapXml);
    Console.WriteLine($"Received {rates.Length} provider groups");

    // Process rates...
    foreach (var providerGroup in rates)
    {
        Console.WriteLine($"Provider: {providerGroup.Provider.Code}");
    }
});

// Listen for historical rates (receives SOAP envelope as XML string)
connection.On<string>("HistoricalRatesUpdated", (soapXml) =>
{
    Console.WriteLine("Historical rates updated (SOAP XML)");
    var rates = DeserializeSoapXml(soapXml);
    Console.WriteLine($"Received {rates.Length} provider groups");
});

await connection.StartAsync();
Console.WriteLine("SignalR Connected");
```

### Authentication

SignalR hub requires JWT authentication (Consumer or Admin role). Pass the JWT token via:
- Query string: `?access_token=your-jwt-token`
- Or using the `accessTokenFactory` option in SignalR client

### Data Format

The SignalR events send data as **XML strings wrapped in SOAP envelopes**, fully consistent with SOAP protocol:

```xml
<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <ArrayOfLatestExchangeRatesGroupedSoap xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <LatestExchangeRatesGroupedSoap>
        <Provider>
          <Id>1</Id>
          <Code>ECB</Code>
          <Name>European Central Bank</Name>
        </Provider>
        <BaseCurrencies>
          <BaseCurrencyGroupSoap>
            <BaseCurrency>EUR</BaseCurrency>
            <Rates>
              <TargetCurrencyRateSoap>
                <TargetCurrency>USD</TargetCurrency>
                <RateInfo>
                  <Rate>1.0850</Rate>
                  <Multiplier>1</Multiplier>
                  <EffectiveRate>1.0850</EffectiveRate>
                </RateInfo>
                <ValidDate>2025-11-07</ValidDate>
                <FetchedAt>2025-11-07T16:00:00Z</FetchedAt>
              </TargetCurrencyRateSoap>
            </Rates>
            <TotalTargetCurrencies>30</TotalTargetCurrencies>
          </BaseCurrencyGroupSoap>
        </BaseCurrencies>
        <TotalBaseCurrencies>1</TotalBaseCurrencies>
        <TotalRates>30</TotalRates>
      </LatestExchangeRatesGroupedSoap>
    </ArrayOfLatestExchangeRatesGroupedSoap>
  </soap:Body>
</soap:Envelope>
```

Clients receive this XML as a **string** and need to parse it using XML parsers (DOMParser in JavaScript, XmlReader/DataContractSerializer in C#).

## Authentication

All SOAP operations except `Login` require JWT authentication.

### Obtaining a Token

1. Call the `Login` operation with credentials
2. Extract the `AccessToken` from the response
3. Include the token in the `Authorization` header for subsequent requests

### Using the Token

For SOAP requests, add the Authorization header to the HTTP request:

**HTTP Header:**
```
Authorization: Bearer eyJhbGc...
```

**C# Example (.NET Client):**
```csharp
using System.ServiceModel;
using System.ServiceModel.Channels;

var client = new CurrencyServiceClient();
using (new OperationContextScope(client.InnerChannel))
{
    var httpRequestProperty = new HttpRequestMessageProperty();
    httpRequestProperty.Headers["Authorization"] = $"Bearer {token}";

    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name]
        = httpRequestProperty;

    var response = await client.GetAllCurrenciesAsync(new GetAllCurrenciesRequest());
}
```

**curl Example:**
```bash
curl -X POST http://localhost:5002/CurrencyService.asmx \
  -H "Content-Type: text/xml; charset=utf-8" \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "SOAPAction: http://tempuri.org/ICurrencyService/GetAllCurrencies" \
  -d '<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <GetAllCurrenciesRequest />
  </soap:Body>
</soap:Envelope>'
```

## Error Handling

All responses include a `Success` field and optional `Fault` information:

**Error Response:**
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <LoginResponse>
      <Success>false</Success>
      <Message>Login failed</Message>
      <Fault>
        <FaultCode>Client</FaultCode>
        <FaultString>Authentication failed</FaultString>
        <Detail>Invalid email or password</Detail>
      </Fault>
    </LoginResponse>
  </soap:Body>
</soap:Envelope>
```

**Fault Codes:**
- `Client`: Client-side error (invalid input, authentication failure)
- `Server`: Server-side error (internal error, database issue)

## Generating Client Code

### .NET (WCF)

```bash
# Using svcutil (WCF)
svcutil http://localhost:5002/CurrencyService.asmx?wsdl /out:CurrencyServiceClient.cs

# Using dotnet-svcutil (.NET Core)
dotnet tool install --global dotnet-svcutil
dotnet-svcutil http://localhost:5002/CurrencyService.asmx?wsdl
```

### Java

```bash
# Using wsimport
wsimport -keep -verbose http://localhost:5002/CurrencyService.asmx?wsdl
```

### Python

```bash
# Using zeep
pip install zeep

# In Python code:
from zeep import Client
client = Client('http://localhost:5002/CurrencyService.asmx?wsdl')
```

### PHP

```php
// Using SoapClient
$client = new SoapClient('http://localhost:5002/CurrencyService.asmx?wsdl');
$result = $client->GetAllCurrencies([]);
```

## Testing

### Testing with SoapUI

1. Create new SOAP project in SoapUI
2. Import WSDL: `http://localhost:5002/AuthenticationService.asmx?wsdl`
3. Test the Login operation
4. Copy the AccessToken from the response
5. Add Authorization header to subsequent requests

### Testing with Postman

1. Create new request
2. Set method to POST
3. URL: `http://localhost:5002/AuthenticationService.asmx`
4. Add header: `Content-Type: text/xml; charset=utf-8`
5. Add header: `SOAPAction: http://tempuri.org/IAuthenticationService/Login`
6. Body (raw XML):
```xml
<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <LoginRequest>
      <Email>admin@example.com</Email>
      <Password>simple</Password>
    </LoginRequest>
  </soap:Body>
</soap:Envelope>
```

### Testing with curl

```bash
# Login
curl -X POST http://localhost:5002/AuthenticationService.asmx \
  -H "Content-Type: text/xml; charset=utf-8" \
  -H "SOAPAction: http://tempuri.org/IAuthenticationService/Login" \
  -d '<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <LoginRequest>
      <Email>admin@example.com</Email>
      <Password>simple</Password>
    </LoginRequest>
  </soap:Body>
</soap:Envelope>'
```

## Project Structure

```
SOAP/
├── Services/                        # SOAP service implementations
│   ├── AuthenticationService.cs    # Authentication operations
│   ├── ExchangeRateService.cs      # Exchange rate operations
│   ├── CurrencyService.cs          # Currency management
│   ├── ProviderService.cs          # Provider management
│   ├── UserService.cs              # User management
│   ├── IAuthenticationService.cs   # SOAP contracts
│   ├── IExchangeRateService.cs
│   ├── ICurrencyService.cs
│   ├── IProviderService.cs
│   ├── IUserService.cs
│   └── SignalRExchangeRatesNotificationService.cs  # SignalR notifications
├── Hubs/                            # SignalR hubs
│   └── ExchangeRatesHub.cs         # Real-time updates hub
├── Models/                          # SOAP data contracts
│   ├── Common/                      # Shared models
│   ├── ExchangeRates/              # Exchange rate models
│   ├── Currencies/                 # Currency models
│   ├── Providers/                  # Provider models
│   └── Users/                      # User models
├── Converters/                      # DTO ↔ SOAP converters
│   ├── ExchangeRateSoapConverters.cs
│   ├── CurrencySoapConverters.cs
│   ├── ProviderSoapConverters.cs
│   └── UserSoapConverters.cs
├── Program.cs                       # Server entry point
├── appsettings.json                 # Configuration
└── appsettings.Development.json     # Development overrides
```

## Default Users

Two users are seeded on startup (if using database seeding):

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

**Cause:** Background jobs (Hangfire) fetching historical data while SOAP requests try to access the database.

**Solutions:**
1. Wait for background jobs to complete (usually 30-60 seconds after startup)
2. Use SQL Server or PostgreSQL for production (configured in `appsettings.json`)
3. Set `UseInMemoryDatabase: false` in configuration to use SQL Server

This limitation affects REST, gRPC, and SOAP APIs equally.

## Comparison with REST and gRPC

| Feature | SOAP API | REST API | gRPC API |
|---------|----------|----------|----------|
| Protocol | SOAP 1.1 | HTTP/1.1 | HTTP/2 |
| Serialization | XML (DataContract) | JSON | Protocol Buffers |
| Contract | WSDL | OpenAPI/Swagger | .proto files |
| Real-time | SignalR (WebSocket) | SignalR (WebSocket) | Server Streaming |
| Authentication | JWT (HTTP Header) | JWT (Bearer token) | JWT (Metadata header) |
| Business Logic | Shared via MediatR | Shared via MediatR | Shared via MediatR |
| Port | 5002/7002 | 5000/5001 | 5001 |
| Client Generation | WCF, wsimport, etc. | OpenAPI Generator | protoc |
| Browser Support | Limited | Full | Limited (requires gRPC-Web) |
| Payload Size | Largest (XML) | Medium (JSON) | Smallest (Binary) |
| Tooling | SoapUI, Postman | Swagger UI, Postman | grpcurl, BloomRPC |

## Production Considerations

### Database

Switch from in-memory SQLite to a production database:

```json
{
  "Database": {
    "UseInMemoryDatabase": false
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=ExchangeRates;..."
  }
}
```

### HTTPS/TLS

Configure HTTPS in production. The SOAP API supports HTTPS on port 7002 by default.

For custom certificates, update `Program.cs`:

```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5002, o => o.UseConnectionLogging());
    options.ListenAnyIP(7002, o =>
    {
        o.UseHttps("certificate.pfx", "password");
    });
});
```

### WS-Security

For production deployments, consider implementing WS-Security standards:
- WS-Security headers for message-level security
- X.509 certificates for authentication
- XML encryption for sensitive data
- XML digital signatures for message integrity

**Note:** The current implementation uses JWT via HTTP headers for simplicity. WS-Security implementation would require additional libraries and configuration.

### Rate Limiting

Configure rate limiting in `appsettings.json`:

```json
{
  "RateLimiting": {
    "EnableRateLimiting": true,
    "PermitLimit": 60,
    "Window": "00:01:00"
  }
}
```

## Troubleshooting

### "Connection refused"

Ensure the server is running on the correct port:
```bash
dotnet run --project ApiLayer/SOAP
```

### "The WSDL document could not be accessed"

Verify the URL includes `?wsdl`:
```
http://localhost:5002/AuthenticationService.asmx?wsdl
```

### "Unauthorized" or "Unauthenticated"

Ensure you're passing the JWT token in the Authorization header:
```
Authorization: Bearer <token>
```

### "Client" Fault

Check your request XML for:
- Missing required fields
- Invalid data types
- Incorrect namespaces

### "Server" Fault

Check server logs for details. Common causes:
- Database connectivity issues
- Invalid JWT configuration
- Background job conflicts (SQLite locking)

## Advantages of SOAP

- **Strong typing**: WSDL provides complete service contracts
- **Automatic client generation**: Tools can generate strongly-typed clients
- **Enterprise integration**: Many enterprise systems have built-in SOAP support
- **Standardized error handling**: SOAP faults provide structured error information
- **Platform independence**: Works with .NET, Java, Python, PHP, and more

## Limitations

- **Larger payload size**: XML is more verbose than JSON or Protocol Buffers
- **Slower performance**: XML parsing is slower than binary protocols
- **Limited browser support**: Modern browsers don't have native SOAP clients
- **Hybrid architecture**: Real-time updates require SignalR connection in addition to SOAP operations

## When to Use SOAP vs REST/gRPC

**Use SOAP when:**
- Integrating with legacy enterprise systems that require SOAP
- Strong typing and contract-first design are critical
- You need automatic client code generation from WSDL
- Working in environments with existing SOAP infrastructure

**Use REST when:**
- Building web applications or mobile apps
- You need human-readable messages (JSON)
- Browser compatibility is important
- Simpler architecture preferred (SignalR available in both REST and SOAP)

**Use gRPC when:**
- Performance is critical
- You need efficient binary serialization
- Server streaming for real-time updates is required
- Building microservices communication

## Further Reading

- [SoapCore Documentation](https://github.com/DigDes/SoapCore)
- [SOAP Specification](https://www.w3.org/TR/soap/)
- [WSDL Guide](https://www.w3.org/TR/wsdl)
- [WS-Security](https://en.wikipedia.org/wiki/WS-Security)
- [DataContractSerializer](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.serialization.datacontractserializer)

## Support

For issues or questions:
1. Check server logs for detailed error messages
2. Verify WSDL is accessible at `{endpoint}?wsdl`
3. Test with SoapUI or Postman to isolate client issues
4. Ensure JWT token is included in Authorization header
5. Check that the database is accessible and not locked

## License

[Your License Here]
