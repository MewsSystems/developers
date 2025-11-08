# ConsoleTestApp - Exchange Rate Management System Testing Client

A comprehensive interactive console application for testing and validating all API endpoints across REST, SOAP, and gRPC protocols in the Exchange Rate Management System.

[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-blue)](https://dotnet.microsoft.com/)
[![Protocol Coverage](https://img.shields.io/badge/Protocol%20Coverage-100%25-brightgreen)](https://github.com)
[![API Parity](https://img.shields.io/badge/API%20Parity-41%20Operations-brightgreen)](https://github.com)

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Quick Start](#quick-start)
- [Configuration](#configuration)
- [Available Commands](#available-commands)
- [Authentication](#authentication)
- [Testing Protocols](#testing-protocols)
- [Real-Time Streaming](#real-time-streaming)
- [Command Reference](#command-reference)
- [Performance Metrics](#performance-metrics)
- [Troubleshooting](#troubleshooting)
- [Architecture](#architecture)

---

## Overview

The ConsoleTestApp is an interactive command-line testing client that provides **100% coverage** of all API operations across three different protocols:

- **REST API** - HTTP/JSON with SignalR streaming
- **SOAP API** - WCF services with SignalR streaming
- **gRPC API** - Protocol Buffers with native server-side streaming

### Key Statistics

- **41 API Operations** testable across all protocols
- **48 Interface Methods** in unified `IApiClient`
- **3 Protocol Implementations** with identical functionality
- **Real-time Streaming** support for all protocols
- **Performance Metrics** tracking for every request

---

## Features

### ✨ Core Capabilities

- 🔄 **Multi-Protocol Support** - Switch seamlessly between REST, SOAP, and gRPC
- 🔐 **JWT Authentication** - Secure bearer token authentication
- 📊 **Performance Metrics** - Track response times and payload sizes
- 🎯 **Interactive Testing** - Auto-complete, command history, colorized output
- 📡 **Real-Time Streaming** - SignalR (REST/SOAP) and gRPC streaming support
- 🎨 **Rich UI** - Spectre.Console powered interface with tables and progress indicators
- 🎭 **Entertainment Mode** - Random API facts during testing sessions
- 📝 **Comprehensive Logging** - Detailed operation tracking

### 🛡️ Security Features

- Admin and Consumer role support
- Token-based authentication across all protocols
- Secure credential management via appsettings.json

### 📈 Monitoring & Diagnostics

- Response time measurement (milliseconds)
- Payload size tracking (bytes)
- Success/failure status for each operation
- System health monitoring
- Error tracking and fetch activity logs

---

## Quick Start

### Prerequisites

- .NET 9.0 SDK or later
- Running instances of REST, SOAP, and gRPC APIs
- Valid user credentials (Admin or Consumer)

### Installation

1. **Navigate to ConsoleTestApp directory:**
   ```bash
   cd TestLayer/ConsoleTestApp
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build the application:**
   ```bash
   dotnet build
   ```

4. **Run the application:**
   ```bash
   dotnet run
   ```

### First Run Experience

On startup, you'll see:
- ASCII art banner
- API facts entertainment (300 seconds)
- Command prompt with auto-complete

Type `help` to see all available commands.

---

## Configuration

### appsettings.json

Configure API endpoints and credentials in `appsettings.json`:

```json
{
  "ApiEndpoints": {
    "Rest": "http://localhost:5188",
    "RestHub": "http://localhost:5188/hubs/exchange-rates",
    "Soap": "http://localhost:5002",
    "Grpc": "http://localhost:5001"
  },
  "TestCredentials": {
    "Admin": {
      "Email": "admin@example.com",
      "Password": "simple"
    },
    "Consumer": {
      "Email": "consumer@example.com",
      "Password": "simple"
    }
  },
  "Entertainment": {
    "StartupDurationSeconds": 300,
    "FactIntervalSeconds": 5,
    "Enabled": true
  },
  "Display": {
    "ShowResponseTimes": true,
    "ShowPayloadSizes": true,
    "ShowDataDiffs": true,
    "CompactMode": false
  }
}
```

### Configuration Options

| Section | Key | Description | Default |
|---------|-----|-------------|---------|
| ApiEndpoints | Rest | REST API base URL | http://localhost:5188 |
| ApiEndpoints | RestHub | SignalR hub for REST | http://localhost:5188/hubs/exchange-rates |
| ApiEndpoints | Soap | SOAP API base URL | http://localhost:5002 |
| ApiEndpoints | Grpc | gRPC API base URL | http://localhost:5001 |
| Entertainment | Enabled | Show random facts | true |
| Entertainment | StartupDurationSeconds | Facts display duration | 300 |
| Display | ShowResponseTimes | Display response metrics | true |
| Display | CompactMode | Minimize output | false |

---

## Available Commands

### General Commands

| Command | Aliases | Description |
|---------|---------|-------------|
| `help` | `?`, `h` | Show all available commands |
| `exit` | `quit`, `q` | Exit the application |
| `clear` | `cls` | Clear the console screen |
| `status` | - | Show current connection status |
| `check-api` | `ping` | Test if API is available |

### Authentication

| Command | Arguments | Description | Example |
|---------|-----------|-------------|---------|
| `login` | `<protocol> <role>` | Authenticate with API | `login rest admin` |
| `logout` | - | Clear authentication token | `logout` |

**Supported Roles:** `admin`, `consumer`

### Exchange Rates (9 operations)

| Command | Arguments | Description | Admin Only |
|---------|-----------|-------------|------------|
| `current` | `<protocol>` | Get current rates (flat) | No |
| `current-grouped` | `<protocol>` | Get current rates (grouped) | No |
| `latest` | `<protocol>` | Get all latest rates | No |
| `latest-rate` | `<protocol> <source> <target> [providerId]` | Get specific currency pair rate | No |
| `historical` | `<protocol> <from> <to>` | Get historical rates | No |
| `convert` | `<protocol> <from> <to> <amount>` | Convert currency | No |
| `stream-start` | `<protocol>` | Start real-time streaming | No |
| `stream-stop` | - | Stop streaming | No |

**Example:**
```bash
latest-rate rest EUR USD
latest-rate grpc EUR USD 1
```

### Currencies (5 operations)

| Command | Arguments | Description | Admin Only |
|---------|-----------|-------------|------------|
| `currencies` | `<protocol>` | List all currencies | No |
| `currency` | `<protocol> <code>` | Get currency by code | No |
| `currency-id` | `<protocol> <id>` | Get currency by ID | No |
| `create-currency` | `<protocol> <code>` | Create new currency | ✅ Yes |
| `delete-currency` | `<protocol> <code>` | Delete currency | ✅ Yes |

**Example:**
```bash
currencies rest
currency soap EUR
create-currency grpc GBP
```

### Providers (14 operations)

| Command | Arguments | Description | Admin Only |
|---------|-----------|-------------|------------|
| `providers` | `<protocol>` | List all providers | No |
| `provider` | `<protocol> <code>` | Get provider by code | No |
| `provider-id` | `<protocol> <id>` | Get provider by ID | No |
| `provider-health` | `<protocol> <code>` | Get provider health status | No |
| `provider-stats` | `<protocol> <code>` | Get provider statistics | No |
| `provider-config` | `<protocol> <code>` | Get provider configuration | ✅ Yes |
| `activate-provider` | `<protocol> <code>` | Activate provider | ✅ Yes |
| `deactivate-provider` | `<protocol> <code>` | Deactivate provider | ✅ Yes |
| `reset-provider-health` | `<protocol> <code>` | Reset health metrics | ✅ Yes |
| `trigger-fetch` | `<protocol> <code>` | Trigger manual fetch | ✅ Yes |
| `create-provider` | `<protocol> <name> <code> <url> <baseCurrencyId> <requiresAuth> [apiKeyRef]` | Create provider | ✅ Yes |
| `update-provider-config` | `<protocol> <code> <name> <url> <requiresAuth> [apiKeyRef]` | Update configuration | ✅ Yes |
| `delete-provider` | `<protocol> <code> <force>` | Delete provider | ✅ Yes |
| `reschedule-provider` | `<protocol> <code>` | Reschedule fetch job | ✅ Yes |

**Example:**
```bash
providers rest
provider-health soap ECB
trigger-fetch grpc CNB
```

### Users (10 operations)

| Command | Arguments | Description | Admin Only |
|---------|-----------|-------------|------------|
| `users` | `<protocol>` | List all users | ✅ Yes |
| `user` | `<protocol> <id>` | Get user by ID | ✅ Yes |
| `user-by-email` | `<protocol> <email>` | Get user by email | ✅ Yes |
| `users-by-role` | `<protocol> <role>` | Get users by role | ✅ Yes |
| `check-email` | `<protocol> <email>` | Check if email exists | ✅ Yes |
| `create-user` | `<protocol> <email> <password> <firstName> <lastName> <role>` | Create user | ✅ Yes |
| `update-user` | `<protocol> <id> <firstName> <lastName>` | Update user info | ✅ Yes |
| `change-password` | `<protocol> <id> <current> <new>` | Change password | ✅ Yes |
| `change-user-role` | `<protocol> <id> <newRole>` | Change user role | ✅ Yes |
| `delete-user` | `<protocol> <id>` | Delete user | ✅ Yes |

**Example:**
```bash
users rest
check-email soap test@example.com
create-user grpc john@example.com Pass123! John Doe Consumer
```

### System Health (3 operations)

| Command | Arguments | Description | Admin Only |
|---------|-----------|-------------|------------|
| `system-health` | `<protocol>` | Get system health status | No |
| `errors` | `<protocol> [count] [severity]` | Get recent errors | ✅ Yes |
| `fetch-activity` | `<protocol> [count] [providerId] [failedOnly]` | Get fetch activity | No |

**Example:**
```bash
system-health rest
errors soap 20 Error
fetch-activity grpc 10 1 true
```

### Testing Commands

| Command | Arguments | Description |
|---------|-----------|-------------|
| `compare` | `<operation> [args...]` | Compare all protocols |
| `solo` | `<protocol> <operation> [args...]` | Test single protocol |
| `test-all` | - | Run all API tests |

**Example:**
```bash
compare currencies
solo rest currencies
```

---

## Authentication

### Login Process

1. **Choose protocol and role:**
   ```bash
   login rest admin
   ```

2. **Credentials are loaded from appsettings.json automatically**

3. **JWT token is obtained and cached**

4. **Token is included in all subsequent requests**

### Role-Based Access

#### Admin Role
- Full access to all operations
- Can create, update, delete resources
- Can manage users and providers
- Can view system diagnostics

#### Consumer Role
- Read-only access to exchange rates
- Can view currencies and providers
- Cannot modify system state

### Token Management

- Tokens are stored in memory for the session
- Use `logout` to clear the authentication token
- Re-login if token expires (401 Unauthorized)

---

## Testing Protocols

### REST API Testing

**Characteristics:**
- HTTP/JSON communication
- RESTful URL patterns
- HTTP status codes for errors
- SignalR for streaming

**Example Session:**
```bash
> login rest admin
✓ Successfully authenticated as admin@example.com

> currencies rest
Fetching currencies from REST...
Response Time: 125ms | Payload: 2,048 bytes | Success: ✓

ID    Code    Name              Symbol    Active
1     EUR     Euro              €         Yes
2     USD     US Dollar         $         Yes
3     GBP     Pound Sterling    £         Yes
```

### SOAP API Testing

**Characteristics:**
- XML/SOAP communication
- WSDL-based contracts
- SoapFault for errors
- SignalR for streaming

**Example Session:**
```bash
> login soap admin
✓ Successfully authenticated as admin@example.com

> provider-health soap ECB
Fetching provider health from SOAP...
Response Time: 89ms | Payload: 512 bytes | Success: ✓

Provider Code: ECB
Provider Name: European Central Bank
Is Healthy: Yes
Consecutive Failures: 0
Last Successful Fetch: 2025-11-08 14:30:22
```

### gRPC API Testing

**Characteristics:**
- Protocol Buffers serialization
- HTTP/2 communication
- Strongly-typed contracts
- Native server-side streaming

**Example Session:**
```bash
> login grpc admin
✓ Successfully authenticated as admin@example.com

> latest-rate grpc EUR USD 1
Fetching latest rate EUR/USD from gRPC...
Response Time: 45ms | Payload: 256 bytes | Success: ✓

Provider: ECB
EUR → USD: 1.0850
Valid Date: 2025-11-08
```

---

## Real-Time Streaming

### Starting Streaming

**REST/SOAP (SignalR):**
```bash
> stream-start rest
Connecting to SignalR hub at http://localhost:5188/hubs/exchange-rates...
✓ Connected to Exchange Rates Hub
ConnectionId: abc-123-def-456
Listening for rate updates...

[14:30:45] Received update: 15 rates from ECB
[14:31:12] Received update: 8 rates from CNB
```

**gRPC (Native Streaming):**
```bash
> stream-start grpc
Starting gRPC server-side streaming...
✓ Connected to StreamExchangeRateUpdates
Listening for rate updates...

[14:30:45] Received update: 15 rates from ECB
[14:31:12] Received update: 8 rates from CNB
```

### Stopping Streaming

```bash
> stream-stop
Stopping stream...
✓ Disconnected from streaming service
```

### Stream Events

**REST/SOAP SignalR Events:**
- `Connected` - Initial connection established
- `LatestRatesUpdated` - New latest rates available
- `HistoricalRatesUpdated` - Historical rates updated

**gRPC Stream:**
- Continuous server-side push
- Automatic reconnection on network errors
- Graceful cancellation support

---

## Command Reference

### Protocol Selection

All commands that interact with APIs require a protocol argument:

| Protocol | Value | API Type |
|----------|-------|----------|
| REST | `rest` | HTTP/JSON |
| SOAP | `soap` | WCF/XML |
| gRPC | `grpc` | Protocol Buffers |

### Date Format

For historical rate queries, use ISO 8601 format:

```bash
historical rest 2025-01-01 2025-01-31
```

### Currency Codes

Use ISO 4217 3-letter currency codes:

- EUR (Euro)
- USD (US Dollar)
- GBP (British Pound)
- CZK (Czech Koruna)
- RON (Romanian Leu)

### Provider Codes

Built-in provider codes:

- **ECB** - European Central Bank
- **CNB** - Czech National Bank
- **BNR** - Romanian National Bank

---

## Performance Metrics

### Displayed Metrics

Every API operation shows:

```
Response Time: 125ms | Payload: 2,048 bytes | Success: ✓
```

**Components:**

| Metric | Description | Unit |
|--------|-------------|------|
| Response Time | Total round-trip time | Milliseconds |
| Payload Size | Response body size | Bytes |
| Success | Operation status | ✓ / ✗ |

### Performance Comparison

Use `compare` command to see protocol performance side-by-side:

```bash
> compare currencies

REST:     125ms | 2,048 bytes | ✓
SOAP:     189ms | 3,456 bytes | ✓
gRPC:      67ms | 1,024 bytes | ✓
```

---

## Troubleshooting

### Common Issues

#### 1. Connection Refused

**Problem:** Cannot connect to API endpoint

**Solution:**
- Verify API is running: `check-api rest`
- Check `appsettings.json` URLs
- Ensure firewall allows connections

#### 2. 401 Unauthorized

**Problem:** Authentication failed

**Solution:**
```bash
# Re-authenticate
logout
login rest admin
```

#### 3. 403 Forbidden

**Problem:** Insufficient permissions

**Solution:**
- Verify you're using Admin role: `login rest admin`
- Consumer role has limited access

#### 4. Stream Connection Fails

**Problem:** Cannot connect to streaming service

**Solution:**
- Check SignalR hub URL in appsettings.json
- Verify API supports streaming
- Ensure WebSocket connections are allowed

#### 5. Timeout Errors

**Problem:** Request takes too long

**Solution:**
- Check network connectivity
- Verify API service is responsive
- Use `system-health` command to check API status

### Debug Information

Enable detailed logging by checking operation metrics:

```bash
> status
Current Session Status:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Protocol:     REST
Authenticated: Yes
User:         admin@example.com
Role:         Admin
Token Expires: 2025-11-08 16:00:00
Streaming:    Inactive
```

---

## Architecture

### Project Structure

```
ConsoleTestApp/
├── Clients/
│   ├── RestApiClient.cs      # REST implementation
│   ├── SoapApiClient.cs      # SOAP implementation
│   └── GrpcApiClient.cs      # gRPC implementation
├── Core/
│   ├── IApiClient.cs         # Unified interface
│   └── ApiClientFactory.cs   # Client factory
├── Models/
│   ├── ExchangeRateData.cs
│   ├── CurrencyData.cs
│   ├── ProviderData.cs
│   ├── UserData.cs
│   └── ApiCallMetrics.cs
├── UI/
│   ├── InteractiveConsole.cs # Main UI loop
│   ├── CommandParser.cs      # Command parsing
│   └── DisplayUtilities.cs   # Output formatting
├── Config/
│   └── AppSettings.cs        # Configuration models
└── Program.cs                # Entry point
```

### Design Patterns

**Factory Pattern:**
- `ApiClientFactory` creates protocol-specific clients

**Strategy Pattern:**
- `IApiClient` interface with three implementations

**Adapter Pattern:**
- Protocol-specific clients adapt to unified interface

**Observer Pattern:**
- Real-time streaming with event callbacks

### Technology Stack

| Component | Technology |
|-----------|-----------|
| Framework | .NET 9.0 |
| HTTP Client | System.Net.Http |
| SOAP Client | SoapCore, WCF |
| gRPC Client | Grpc.Net.Client |
| SignalR Client | Microsoft.AspNetCore.SignalR.Client |
| Console UI | Spectre.Console |
| Serialization | System.Text.Json, Protobuf |

---

## Advanced Usage

### Batch Testing

Test multiple operations sequentially:

```bash
> login rest admin
> currencies rest
> providers rest
> system-health rest
> logout
```

### Protocol Comparison

Compare all protocols for same operation:

```bash
> compare currencies
> compare provider-health ECB
> compare system-health
```

### Automated Scripts

Create a `.txt` file with commands:

```text
login rest admin
currencies rest
providers rest
system-health rest
logout
exit
```

Run via input redirection:
```bash
dotnet run < test-script.txt
```

### Performance Testing

Measure response times across protocols:

```bash
> compare latest
REST:  125ms
SOAP:  189ms
gRPC:   67ms
```

---

## API Coverage

### Complete Operation List

The ConsoleTestApp provides 100% coverage of all API operations:

✅ **Authentication** (2 operations)
- Login
- Logout (client-side)

✅ **Exchange Rates** (9 operations)
- Get current rates (flat)
- Get current rates (grouped)
- Get latest rate (single pair)
- Get all latest rates
- Get historical rates
- Convert currency
- Stream updates (start)
- Stream updates (stop)

✅ **Currencies** (5 operations)
- List all currencies
- Get currency by ID
- Get currency by code
- Create currency
- Delete currency

✅ **Providers** (14 operations)
- List all providers
- Get provider by ID
- Get provider by code
- Get provider health
- Get provider statistics
- Get provider configuration
- Activate provider
- Deactivate provider
- Reset provider health
- Trigger manual fetch
- Create provider
- Update provider configuration
- Delete provider
- Reschedule provider

✅ **Users** (10 operations)
- List all users
- Get user by ID
- Get user by email
- Get users by role
- Check email exists
- Create user
- Update user
- Change password
- Change user role
- Delete user

✅ **System Health** (3 operations)
- Get system health
- Get recent errors
- Get fetch activity

**Total: 41 API Operations** testable across all protocols!

---

## Contributing

### Extending the Client

To add a new operation:

1. **Add method to `IApiClient` interface:**
   ```csharp
   Task<(DataType Data, ApiCallMetrics Metrics)> NewOperationAsync(params);
   ```

2. **Implement in all three clients:**
   - `RestApiClient.cs`
   - `SoapApiClient.cs`
   - `GrpcApiClient.cs`

3. **Add command to `CommandType` enum:**
   ```csharp
   public enum CommandType {
       ...
       NewOperation
   }
   ```

4. **Add command mapping in `CommandParser`:**
   ```csharp
   "new-operation" => new ParsedCommand { Type = CommandType.NewOperation }
   ```

5. **Add handler in `InteractiveConsole`:**
   ```csharp
   private async Task HandleNewOperationAsync(string[] args) { ... }
   ```

### Testing Guidelines

- Test all three protocols for each operation
- Verify authentication requirements
- Check error handling
- Validate response data
- Measure performance metrics

---

## License

This testing client is part of the Exchange Rate Management System.

---

## Support

For issues or questions:
- Check [Troubleshooting](#troubleshooting) section
- Review [Parity.txt](../../Parity.txt) for API coverage details
- Consult API-specific documentation

---

## Version History

### v1.0 (2025-11-08)
- Initial release
- Full REST, SOAP, gRPC support
- 41 API operations
- Real-time streaming
- Interactive console UI
- Performance metrics tracking
- Authorization consistency fixes

---

**Happy Testing! 🚀**
