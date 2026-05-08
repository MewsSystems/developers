# Mews.ExchangeRateMonitor — Backend task (CNB exchange rates)

A compact, production-grade implementation of the Mews backend task: fetch daily exchange rates from the Czech National Bank (CNB), cache them, and expose them via a clean application slice.

This repo is organized into small projects with clear boundaries (Domain / Features / Infrastructure / Host) and uses .NET 9, Minimal APIs, FluentValidation, Serilog, central package management, and in-memory caching. The solution also includes a ready-to-use Seq instance via Docker for structured logging.

# Solution structure

- Mews.ExchangeRateMonitor.Host/**: Entry point for the application that configures and runs modules
- Modules/ExchangeRate/**: Exchange rate feature module with API endpoints, handlers, and validation
- Common/**: Shared abstractions, extensions, and utilities

# Tech stack

- .NET 9, C# 13
- ASP.NET Core Minimal APIs
- FluentValidation for input validation
- IHttpClientFactory for outbound calls (typed/named client to CNB)
- IMemoryCache for response caching with date-aware TTL
- Serilog with optional Seq sink (Docker)
- Central Package Management (Directory.Packages.props) for consistent versions

# Running the project

1. Ensure Docker and Docker Compose are installed
2. Navigate to the project root directory
3. Run `docker-compose up -d`
4. Access the API at http://localhost:5000/swagger
5. Access Seq dashboard at http://localhost:8081
6. Access Jaeger UI at http://localhost:16686
7. Ensure your dockercompose runs

## Architecture

### Vertical Slices with Clean Architecture

The project uses a combination of Vertical Slices and Clean Architecture

- **Vertical Slices**: Features are organized by business functionality rather than technical layers 
- **Clean Architecture**: Core domain is isolated from infrastructure concerns

### Key Architecture Decisions
- **Manual Handlers**: Simple handler pattern used instead of MediatR to reduce unnecessary abstractions
- **Manual Mapping**: Direct mapping between DTOs and domain objects for simplicity and control
- **Result Pattern**: Custom implementation that returns success or detailed errors without exceptions
- **Fluent Validation**: Provides clear, strongly-typed validation rules for all inputs

### Observability
- **OpenTelemetry (OTEL)**: Instrument code for metrics, logs, and traces - export to Jaeger for distributed tracing
- **Seq**: Centralized structured logging  with persistent volume mounted at ./docker_data/seq
- **Jaeger**: Visualize request flows


