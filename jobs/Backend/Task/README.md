# Exchange Rate API

A production-ready REST API for fetching real-time exchange rates from the Czech National Bank (CNB). Built with .NET 8.0, featuring caching, resilience patterns, comprehensive API documentation, and Docker support.

## 📋 Table of Contents

- [Features](#-features)
- [Quick Start](#-quick-start)
- [API Endpoints](#-api-endpoints)
- [Running the Application](#-running-the-application)
- [Docker Deployment](#-docker-deployment)
- [Configuration](#️-configuration)
- [API Examples](#-api-examples)
- [Architecture](#️-architecture)
- [Testing](#-testing)
- [Production Deployment](#-production-deployment)
- [Troubleshooting](#-troubleshooting)

---

## ✨ Features

- **RESTful API** with 4 endpoints for exchange rate operations
- **Swagger/OpenAPI** documentation with interactive UI
- **Smart Caching** - 60-minute cache to reduce API calls (configurable)
- **Resilience Patterns** - Retry logic, timeouts, and circuit breaker
- **Health Checks** - Monitoring endpoint for orchestration
- **Docker Support** - Multi-stage builds with security best practices
- **Comprehensive Logging** - Structured logging with correlation
- **Input Validation** - Proper error handling and HTTP status codes
- **Production Ready** - Non-root user, health checks, configurable settings
- **43 Tests** - 22 unit tests + 21 integration tests, all passing ✅

### Technical Stack

- **.NET 8.0** (LTS)
- **ASP.NET Core** Minimal APIs
- **Polly** for resilience
- **Swashbuckle** for API documentation
- **xUnit** for testing
- **FluentAssertions** for readable test assertions
- **Docker** for containerization

---

## 🚀 Quick Start

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) (optional)

### Run Locally (Fastest)

```bash
cd jobs\Backend\Task
dotnet run
```

The API will start at:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001

Open **http://localhost:5000** in your browser to access the **Swagger UI** 🎉

---

## 📡 API Endpoints

### 1. Get Exchange Rates (GET)

Fetch exchange rates for specific currencies using query parameters.

**Endpoint:** `GET /api/exchange-rates?currencies=USD,EUR,GBP`

**Example:**
```bash
curl "http://localhost:5000/api/exchange-rates?currencies=USD,EUR,GBP"
```

**Response:**
```json
[
  {
    "sourceCurrency": "USD",
    "targetCurrency": "CZK",
    "rate": 21.216
  },
  {
    "sourceCurrency": "EUR",
    "targetCurrency": "CZK",
    "rate": 24.375
  },
  {
    "sourceCurrency": "GBP",
    "targetCurrency": "CZK",
    "rate": 28.123
  }
]
```

---

### 2. Get Exchange Rates (POST)

Fetch exchange rates using a JSON body (useful for large currency lists).

**Endpoint:** `POST /api/exchange-rates`

**Request Body:**
```json
{
  "currencyCodes": ["USD", "EUR", "GBP", "JPY", "CHF"]
}
```

**Example:**
```bash
curl -X POST "http://localhost:5000/api/exchange-rates" \
  -H "Content-Type: application/json" \
  -d '{"currencyCodes": ["USD", "EUR", "GBP"]}'
```

**Response:** Same as GET endpoint

---

### 3. Get Supported Currencies

Get a dynamically fetched list of all currencies currently supported by CNB.

**Endpoint:** `GET /api/exchange-rates/supported`

**Example:**
```bash
curl "http://localhost:5000/api/exchange-rates/supported"
```

**Response:**
```json
{
  "baseCurrency": "CZK",
  "supportedCurrencies": [
    "AUD", "BGN", "BRL", "CAD", "CHF", "CNY", "DKK", "EUR",
    "GBP", "HKD", "HUF", "IDR", "ILS", "INR", "ISK", "JPY",
    "KRW", "MXN", "NOK", "NZD", "PHP", "PLN", "RON", "RUB",
    "SEK", "SGD", "THB", "TRY", "USD", "ZAR"
  ],
  "count": 30,
  "note": "This list is dynamically fetched from CNB and cached for performance."
}
```

---

### 4. Health Check

Check if the API is running and healthy.

**Endpoint:** `GET /health`

**Example:**
```bash
curl "http://localhost:5000/health"
```

**Response:**
```json
{
  "status": "Healthy",
  "timestamp": "2025-11-06T10:00:00Z",
  "service": "Exchange Rate API"
}
```

---

## 💻 Running the Application

### Option 1: Development Mode with Auto-Reload

```bash
cd C:\mews-task\mews-backend-task\jobs\Backend\Task
dotnet watch run
```

The app will automatically reload when you make code changes.

---

### Option 2: Build and Run

```bash
# Build
dotnet build -c Release

# Run
dotnet run -c Release
```

---

### Option 3: Using the Published DLL

```bash
# Publish
dotnet publish -c Release -o ./publish

# Run
cd publish
dotnet ExchangeRateUpdater.dll
```

---

## 🐳 Docker Deployment

### Quick Start with Docker Compose (Recommended)

```bash
cd C:\mews-task\mews-backend-task\jobs\Backend\Task

# Start the API
docker-compose up -d

# View logs
docker-compose logs -f

# Stop the API
docker-compose down
```

The API will be available at **http://localhost:5000**

---

### Manual Docker Commands

**Build the image:**
```bash
docker build -t exchange-rate-api .
```

**Run the container:**
```bash
docker run -d \
  --name exchange-rate-api \
  -p 5000:8080 \
  -e CnbExchangeRate__CacheDurationMinutes=60 \
  exchange-rate-api
```

**View logs:**
```bash
docker logs -f exchange-rate-api
```

**Stop and remove:**
```bash
docker stop exchange-rate-api
docker rm exchange-rate-api
```

---

### Docker Image Details

- **Base Image (Build):** `mcr.microsoft.com/dotnet/sdk:8.0`
- **Base Image (Runtime):** `mcr.microsoft.com/dotnet/aspnet:8.0`
- **Exposed Ports:** 8080 (HTTP)
- **Security:** Runs as non-root user (`appuser`)
- **Health Check:** Pings `/health` endpoint every 30 seconds
- **Multi-stage Build:** Optimized for smaller image size

---

## ⚙️ Configuration

### appsettings.json

```json
{
  "CnbExchangeRate": {
    "ApiUrl": "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt",
    "TimeoutSeconds": 30,
    "RetryCount": 3,
    "RetryDelayMilliseconds": 1000,
    "EnableCache": true,
    "CacheDurationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Environment Variables

Override settings using environment variables:

```bash
# Enable/disable cache
export CnbExchangeRate__EnableCache=true

# Cache duration (minutes)
export CnbExchangeRate__CacheDurationMinutes=120

# Timeout for CNB API calls (seconds)
export CnbExchangeRate__TimeoutSeconds=30

# Number of retry attempts
export CnbExchangeRate__RetryCount=5

# Run the app
dotnet run
```

### Docker Environment Variables

```bash
docker run -d \
  -p 5000:8080 \
  -e CnbExchangeRate__EnableCache=true \
  -e CnbExchangeRate__CacheDurationMinutes=120 \
  -e CnbExchangeRate__TimeoutSeconds=30 \
  -e CnbExchangeRate__RetryCount=5 \
  exchange-rate-api
```

---

## 🔍 API Examples

### PowerShell Examples

```powershell
# GET request
Invoke-RestMethod -Uri "http://localhost:5000/api/exchange-rates?currencies=USD,EUR,GBP"

# POST request
$body = @{
    currencyCodes = @("USD", "EUR", "GBP", "JPY")
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/exchange-rates" `
  -Method Post `
  -Body $body `
  -ContentType "application/json"

# Get supported currencies
Invoke-RestMethod -Uri "http://localhost:5000/api/exchange-rates/supported"

# Health check
Invoke-RestMethod -Uri "http://localhost:5000/health"
```

### cURL Examples

```bash
# GET request
curl "http://localhost:5000/api/exchange-rates?currencies=USD,EUR"

# POST request
curl -X POST "http://localhost:5000/api/exchange-rates" \
  -H "Content-Type: application/json" \
  -d '{
    "currencyCodes": ["USD", "EUR", "GBP", "JPY", "CHF"]
  }'

# Get supported currencies
curl "http://localhost:5000/api/exchange-rates/supported"

# Health check
curl "http://localhost:5000/health"
```

### JavaScript/Fetch Example

```javascript
// GET request
const response = await fetch(
  'http://localhost:5000/api/exchange-rates?currencies=USD,EUR,GBP'
);
const rates = await response.json();
console.log(rates);

// POST request
const response = await fetch('http://localhost:5000/api/exchange-rates', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    currencyCodes: ['USD', 'EUR', 'GBP', 'JPY']
  })
});
const rates = await response.json();
console.log(rates);
```

### Python Example

```python
import requests

# GET request
response = requests.get(
    'http://localhost:5000/api/exchange-rates',
    params={'currencies': 'USD,EUR,GBP'}
)
rates = response.json()
print(rates)

# POST request
response = requests.post(
    'http://localhost:5000/api/exchange-rates',
    json={'currencyCodes': ['USD', 'EUR', 'GBP', 'JPY']}
)
rates = response.json()
print(rates)
```

---

## 🏗️ Architecture

### Project Structure

```
Task/
├── Api/                          # API models and endpoints
│   ├── ExchangeRateEndpoints.cs  # API endpoint definitions
│   ├── ExchangeRateRequest.cs    # Request DTOs
│   ├── ExchangeRateResponse.cs   # Response DTOs
│   └── ErrorResponse.cs          # Error DTOs
├── Configuration/                # Configuration classes
│   └── CnbExchangeRateConfiguration.cs
├── Constants/                    # Application constants
│   └── LogMessages.cs            # Log message templates
├── Infrastructure/               # Infrastructure layer
│   ├── CnbApiClient.cs           # HTTP client for CNB API
│   ├── CnbDataParser.cs          # Parse CNB text format
│   ├── ExchangeRateCache.cs      # In-memory caching for rates
│   ├── SupportedCurrenciesCache.cs # Cache for supported currencies
│   ├── ExchangeRateProviderException.cs
│   ├── ServiceCollectionExtensions.cs
│   ├── ICnbApiClient.cs          # Interface
│   ├── ICnbDataParser.cs         # Interface
│   ├── IExchangeRateCache.cs     # Interface
│   └── ISupportedCurrenciesCache.cs # Interface
├── Models/                       # Domain models
│   ├── Currency.cs               # Currency value object (record)
│   ├── ExchangeRate.cs           # Exchange rate value object (record)
│   └── CnbExchangeRateDto.cs     # CNB data transfer object
├── Services/                     # Business logic
│   └── ExchangeRateProvider.cs   # Main service
├── Program.cs                    # Application entry point
├── GlobalUsings.cs               # Global using statements
├── appsettings.json              # Configuration
├── Dockerfile                    # Docker image definition
├── docker-compose.yml            # Docker Compose configuration
└── README.md                     # This file
```

### Design Principles

- **SOLID Principles** - Single responsibility, dependency inversion
- **Clean Architecture** - Separation of concerns (Models, Services, Infrastructure)
- **Dependency Injection** - All dependencies injected via DI container
- **Resilience Patterns** - Retry, timeout, circuit breaker using Polly
- **Caching Strategy** - In-memory cache with configurable TTL
- **Error Handling** - Comprehensive exception handling with proper HTTP codes

### Key Components

1. **ExchangeRateProvider** - Main service orchestrating the workflow
2. **CnbApiClient** - HTTP client with resilience patterns (retry, timeout, circuit breaker)
3. **CnbDataParser** - Parses CNB's pipe-delimited text format
4. **ExchangeRateCache** - In-memory cache for exchange rates with TTL
5. **SupportedCurrenciesCache** - Dedicated cache for supported currency codes
6. **ExchangeRateEndpoints** - API endpoint definitions separated from Program.cs
7. **LogMessages** - Centralized log message templates for consistency

### Data Flow

```
Client Request
    ↓
API Endpoint (Program.cs)
    ↓
ExchangeRateProvider (check cache)
    ↓
[Cache Hit] → Return cached data
    ↓
[Cache Miss] → CnbApiClient (with retry/timeout)
    ↓
CNB API (https://www.cnb.cz/...)
    ↓
CnbDataParser (parse text format)
    ↓
ExchangeRateCache (store in cache)
    ↓
Return to client
```

---

## 🧪 Testing

### Run All Tests

```bash
# Unit tests
cd C:\mews-task\mews-backend-task\jobs\Backend\UnitTests
dotnet test

# Integration tests (requires internet connection)
cd C:\mews-task\mews-backend-task\jobs\Backend\IntegrationTests
dotnet test

# Run all tests from solution
cd C:\mews-task\mews-backend-task\jobs\Backend\Task
dotnet build
dotnet test
```

### Test Coverage

**Unit Tests (22 tests):**
- `CnbDataParserTests` - 7 tests for parsing logic and edge cases
- `ExchangeRateProviderTests` - 10 tests for provider logic, caching, and supported currencies
- `ExchangeRateCacheTests` - 5 tests for caching behavior

**Integration Tests (21 tests):**
- `ExchangeRateProviderE2ETests` - 6 tests with real CNB API
- `CachingE2ETests` - 6 tests for end-to-end cache behavior
- `ErrorScenarioE2ETests` - 1 test for cancellation token handling
- `ApiEndpointTests` - 8 tests for HTTP API endpoints (GET, POST, validation)

**Total: 43 tests, all passing ✅**

### Run Tests with Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Run Specific Tests

```bash
# Run only unit tests
dotnet test --filter "FullyQualifiedName~ExchangeRateUpdater.UnitTests"

# Run only integration tests
dotnet test --filter "FullyQualifiedName~ExchangeRateUpdater.IntegrationTests"

# Run tests matching a pattern
dotnet test --filter "FullyQualifiedName~Cache"

# Run specific test class
dotnet test --filter "FullyQualifiedName~ApiEndpointTests"
```

---

## 🚀 Production Deployment

### Recommended Settings

1. **Use HTTPS only** with valid SSL certificates
2. **Implement authentication** (API keys, OAuth, JWT)
3. **Add rate limiting** to prevent abuse
4. **Configure CORS** for web clients
5. **Use distributed cache** (Redis) for multi-instance deployments
6. **Enable application monitoring** (Application Insights, Prometheus)
7. **Set up logging aggregation** (ELK, Seq, Azure Monitor)
8. **Configure health checks** for load balancers

### Production docker-compose.yml

```yaml
version: '3.8'

services:
  exchange-rate-api:
    image: exchange-rate-api:1.0.0
    container_name: exchange-rate-api-prod
    ports:
      - "5000:8080"
    environment:
      - DOTNET_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:8080
      - CnbExchangeRate__EnableCache=true
      - CnbExchangeRate__CacheDurationMinutes=60
      - CnbExchangeRate__TimeoutSeconds=30
      - CnbExchangeRate__RetryCount=3
    restart: always
    healthcheck:
      test: ["CMD", "curl", "--fail", "http://localhost:8080/health"]
      interval: 30s
      timeout: 3s
      retries: 3
      start_period: 10s
    deploy:
      resources:
        limits:
          cpus: '1.0'
          memory: 512M
        reservations:
          cpus: '0.5'
          memory: 256M
    networks:
      - app-network
    logging:
      driver: "json-file"
      options:
        max-size: "10m"
        max-file: "3"

networks:
  app-network:
    driver: bridge
```

### Cloud Deployment Options

**Azure:**
- Azure App Service
- Azure Container Instances
- Azure Kubernetes Service (AKS)

**AWS:**
- AWS Elastic Beanstalk
- AWS ECS/Fargate
- AWS EKS

**Google Cloud:**
- Google App Engine
- Google Cloud Run
- Google Kubernetes Engine (GKE)

---

## 🐛 Troubleshooting

### API Won't Start

**Check port availability:**
```bash
# Windows
netstat -ano | findstr :5000

# Linux/Mac
lsof -i :5000
```

**Solution:** Kill the process or use a different port

```bash
# Use different port
dotnet run --urls "http://localhost:5001"
```

---

### "Service Unavailable" Errors

**Possible causes:**
1. CNB API is down
2. Network connectivity issues
3. Firewall blocking outbound requests

**Test CNB API manually:**
```bash
curl https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt
```

**Solution:** Check network settings, verify firewall rules

---

### Empty Response / No Exchange Rates

**Causes:**
1. Invalid currency codes
2. CNB doesn't support the requested currency
3. Rates not yet published (CNB updates after 2:30 PM CET)

**Solution:**
```bash
# Check supported currencies
curl "http://localhost:5000/api/exchange-rates/supported"

# Verify currency codes are valid (3-letter ISO codes)
```

---

### Docker Container Won't Start

**Check logs:**
```bash
docker logs exchange-rate-api
```

**Common issues:**
- Port conflict → Change port mapping: `-p 5001:8080`
- Image build failed → Rebuild: `docker-compose build --no-cache`
- Health check failing → Wait 10-15 seconds for startup

**Verify health:**
```bash
docker inspect --format='{{.State.Health.Status}}' exchange-rate-api
```

---

### Cache Not Working

**Verify cache is enabled:**
```bash
# Check configuration
cat appsettings.json | grep EnableCache
```

**Test cache behavior:**
```bash
# First call (cache miss - slower)
time curl "http://localhost:5000/api/exchange-rates?currencies=USD"

# Second call (cache hit - faster)
time curl "http://localhost:5000/api/exchange-rates?currencies=USD"
```

The second call should be significantly faster (< 10ms vs 200ms+)

---

## 📚 Common Currency Codes

| Code | Currency | Country/Region |
|------|----------|----------------|
| USD | US Dollar | United States |
| EUR | Euro | Eurozone |
| GBP | British Pound | United Kingdom |
| JPY | Japanese Yen | Japan |
| CHF | Swiss Franc | Switzerland |
| AUD | Australian Dollar | Australia |
| CAD | Canadian Dollar | Canada |
| CNY | Chinese Yuan | China |
| INR | Indian Rupee | India |
| RUB | Russian Ruble | Russia |
| SEK | Swedish Krona | Sweden |
| NOK | Norwegian Krone | Norway |
| DKK | Danish Krone | Denmark |
| PLN | Polish Zloty | Poland |
| THB | Thai Baht | Thailand |
| SGD | Singapore Dollar | Singapore |
| HKD | Hong Kong Dollar | Hong Kong |
| KRW | South Korean Won | South Korea |
| ZAR | South African Rand | South Africa |
| MXN | Mexican Peso | Mexico |

---

## 📝 Notes

- **Base Currency:** All rates are in CZK (Czech Koruna)
- **Cache Duration:** Default 60 minutes (configurable)
- **Invalid Currencies:** Silently omitted from response (no error thrown)
- **Rate Normalization:** Rates provided per 1 unit of source currency (CNB sometimes provides rates per 100 units, which are automatically normalized)

---

## 🤝 Contributing

This is a task implementation for Mews. For the original task, see:
https://github.com/MewsSystems/developers/blob/master/jobs/Backend/DotNet.md

---

