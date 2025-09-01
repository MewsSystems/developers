# Architecture

## Overview

The Exchange Rate Provider is built using Clean Architecture principles to ensure separation of concerns, testability, and maintainability. The architecture is divided into multiple layers, each with specific responsibilities and dependencies.

## Architectural Diagrams

### Clean Architecture Layers

```mermaid
graph TB
    subgraph "Presentation Layer<br/>(Entry Points)"
        API[ExchangeRateProvider.Api<br/>- ExchangeRatesController<br/>- Program.cs<br/>- Swagger/OpenAPI<br/>- Health Checks]
        Console[ExchangeRateProvider.Console<br/>- Program.cs<br/>- Command-line Interface]
    end

    subgraph "Application Layer<br/>(Use Cases & Business Logic)"
        App[ExchangeRateProvider.Application<br/>- GetExchangeRatesQuery<br/>- GetExchangeRatesQueryHandler<br/>- MediatR Pipeline<br/>- ServiceCollectionExtensions]
    end

    subgraph "Domain Layer<br/>(Core Business Rules)"
        Domain[ExchangeRateProvider.Domain<br/>- Currency Entity<br/>- ExchangeRate Entity<br/>- IExchangeRateProvider Interface<br/>- IProviderRegistry Interface<br/>- ProviderRegistry Implementation]
    end

    subgraph "Infrastructure Layer<br/>(External Concerns)"
        Infra[ExchangeRateProvider.Infrastructure<br/>- CnbExchangeRateProvider<br/>- DistributedCachingExchangeRateProvider<br/>- CnbCacheStrategy<br/>- ProviderRegistrationHostedService<br/>- ProviderRegistrationService<br/>- Polly Policies<br/>- ServiceCollectionExtensions]
    end

    subgraph "Tests Layer<br/>(Quality Assurance)"
        Tests[ExchangeRateProvider.Tests<br/>- Unit Tests<br/>- Integration Tests<br/>- Infrastructure Tests<br/>- API E2E Tests]
    end

    subgraph "External Systems"
        CNB[CNB API<br/>https://api.cnb.cz]
        Redis[(Redis Cache<br/>Distributed Storage)]
        Docker[Docker<br/>Containerization]
    end

    API --> App
    Console --> App
    App --> Domain
    Infra --> Domain
    Tests --> App
    Tests --> Domain
    Tests --> Infra
    Infra --> CNB
    Infra --> Redis
    API --> Docker
    Console --> Docker

    style Domain fill:#e1f5fe,stroke:#01579b,stroke-width:3px
    style App fill:#f3e5f5,stroke:#4a148c,stroke-width:2px
    style Infra fill:#e8f5e8,stroke:#1b5e20,stroke-width:2px
    style API fill:#fff3e0,stroke:#e65100,stroke-width:2px
    style Console fill:#fff3e0,stroke:#e65100,stroke-width:2px
    style Tests fill:#fafafa,stroke:#424242,stroke-width:1px
    style CNB fill:#ffebee,stroke:#b71c1c
    style Redis fill:#f3e5f5,stroke:#4a148c
    style Docker fill:#e8f5e8,stroke:#1b5e20
```

### Data Flow Diagram

```mermaid
flowchart TD
    Client[Client Application] -->|HTTP GET /api/exchange-rates| Controller[ExchangeRatesController]
    Controller -->|Validate Request| Controller
    Controller -->|Send Query| MediatR[MediatR Pipeline]
    MediatR -->|Route to Handler| Handler[GetExchangeRatesQueryHandler]
    Handler -->|Request Providers| Registry[ProviderRegistry]
    Registry -->|Select Provider| CacheCheck{Cache Check<br/>CnbCacheStrategy}
    CacheCheck -->|Cache Hit| CacheReturn[Return Cached Data]
    CacheCheck -->|Cache Miss| Provider[CnbExchangeRateProvider]
    Provider -->|Fetch Data| CNBAPI[CNB API<br/>https://api.cnb.cz/cnbapi/exrates/daily]
    CNBAPI -->|Return Rates| Provider
    Provider -->|Apply Caching| DistributedCache[DistributedCachingExchangeRateProvider<br/>Redis]
    DistributedCache -->|Store in Cache| DistributedCache
    DistributedCache -->|Return Data| Handler
    CacheReturn -->|Return Data| Handler
    Handler -->|Process Response| Handler
    Handler -->|Return Result| MediatR
    MediatR -->|Return Result| Controller
    Controller -->|Format Response| Controller
    Controller -->|HTTP Response| Client

    style Client fill:#e3f2fd
    style Controller fill:#fff3e0
    style MediatR fill:#f3e5f5
    style Handler fill:#e8f5e8
    style Registry fill:#e1f5fe
    style CacheCheck fill:#fff9c4
    style Provider fill:#e8f5e8
    style CNBAPI fill:#ffebee
    style DistributedCache fill:#f3e5f5
```

## Layer Descriptions

### Domain Layer
- **Purpose**: Contains core business entities, value objects, and interfaces
- **Components**:
  - `Currency` and `ExchangeRate` entities with validation
  - `IExchangeRateProvider` interface for provider abstraction
  - `IProviderRegistry` for managing multiple providers
- **Dependencies**: None (innermost layer)

### Application Layer
- **Purpose**: Contains business logic and use cases
- **Components**:
  - MediatR-based command/query pattern
  - `GetExchangeRatesQuery` and handler for rate retrieval
- **Dependencies**: Domain Layer

### Infrastructure Layer
- **Purpose**: Handles external concerns and implementations
- **Components**:
  - `CnbExchangeRateProvider`: CNB API integration
  - `CnbCacheStrategy`: Intelligent caching based on CNB publication schedule
  - `DistributedCachingExchangeRateProvider`: Redis-based caching decorator
  - Polly policies for resilience (retry, circuit breaker)
  - Provider registration services
- **Dependencies**: Domain Layer

### Presentation Layer
- **Purpose**: Entry points for the application
- **Components**:
  - **API Layer** (`ExchangeRateProvider.Api`): ASP.NET Core RESTful web API with controllers, Swagger, health checks
  - **Console Layer** (`ExchangeRateProvider.Console`): Command-line interface for testing
- **Dependencies**: Application Layer

### Tests Layer
- **Purpose**: Comprehensive testing coverage
- **Components**:
  - Unit tests for business logic
  - Integration tests for API and external services
  - Infrastructure tests for service registration
- **Dependencies**: All layers

## Key Architectural Decisions

### 1. Clean Architecture
**Decision**: Implement Clean Architecture with strict layer separation.
**Rationale**:
- Ensures separation of concerns
- Improves testability by allowing mocking of dependencies
- Facilitates maintainability and evolution of the codebase
- Prevents business logic from being coupled to external frameworks

### 2. Provider Abstraction Pattern
**Decision**: Use `IExchangeRateProvider` interface with priority-based provider registry.
**Rationale**:
- Allows easy addition of new exchange rate providers
- Enables fallback mechanisms if primary provider fails
- Supports different providers for different currencies or regions
- Maintains single responsibility principle

### 3. Intelligent Caching Strategy
**Decision**: Implement time-based caching that adapts to CNB publication schedule.
**Rationale**:
- Reduces unnecessary API calls to CNB
- Optimizes performance during high-frequency publication windows
- Balances freshness of data with system performance
- Reduces load on external API

### 4. CQRS with MediatR
**Decision**: Use MediatR for implementing CQRS pattern in the Application layer.
**Rationale**:
- Separates read and write operations
- Improves code organization and maintainability
- Enables easy testing of handlers
- Supports cross-cutting concerns like logging and validation

### 5. Resilience with Polly
**Decision**: Implement Polly policies for retry, circuit breaker, and timeout.
**Rationale**:
- Handles transient failures gracefully
- Prevents cascading failures
- Improves system reliability and user experience
- Provides configurable resilience strategies

### 6. Distributed Caching with Redis
**Decision**: Use Redis for distributed caching in multi-instance deployments.
**Rationale**:
- Enables cache sharing across multiple application instances
- Improves performance in scaled environments
- Provides persistence and backup capabilities
- Integrates well with cloud deployments

### 7. Docker Containerization
**Decision**: Provide multi-stage Dockerfiles for development and production.
**Rationale**:
- Ensures consistent deployment across environments
- Simplifies scaling and orchestration
- Improves development workflow
- Enables efficient CI/CD pipelines

### 8. Comprehensive Testing Strategy
**Decision**: Maintain high test coverage with unit, integration, and infrastructure tests.
**Rationale**:
- Ensures code quality and prevents regressions
- Enables confident refactoring and feature additions
- Validates integration with external services
- Supports continuous integration practices

## Data Flow

1. **API Request**: Client sends request to `/api/exchange-rates`
2. **Controller**: `ExchangeRatesController` receives request and validates input
3. **Application**: `GetExchangeRatesQuery` is sent via MediatR
4. **Handler**: `GetExchangeRatesQueryHandler` processes the query
5. **Infrastructure**: Provider registry selects appropriate provider
6. **Caching**: Cache strategy checks for cached data
7. **Provider**: If cache miss, fetches data from CNB API
8. **Response**: Data flows back through layers to client

## Deployment Architecture

The application supports multiple deployment scenarios:

- **Development**: Local Docker Compose with Redis
- **Production**: Containerized deployment with external Redis
- **Console**: Standalone executable for batch operations

## Monitoring and Observability

- Health checks for CNB API and Redis connectivity
- Prometheus metrics for performance monitoring
- Structured logging with Serilog
- Request tracing and correlation IDs