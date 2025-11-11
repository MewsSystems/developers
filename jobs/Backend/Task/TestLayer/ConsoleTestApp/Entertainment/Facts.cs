namespace ConsoleTestApp.Entertainment;

public static class Facts
{
    public static readonly string[] ArchitectureFacts = new[]
    {
        "This solution implements Clean Architecture with 5 distinct layers!",
        "All 3 APIs (REST, SOAP, gRPC) share 100% of business logic via MediatR CQRS!",
        "The Domain Layer uses Aggregate pattern - User is a rich domain object!",
        "Repository Pattern bridges between Domain aggregates and EF Core entities!",
        "The solution uses Adapter Pattern to convert between layers!",
        "Dependency Injection flows inward - Domain has zero dependencies!",
        "FluentValidation validates commands before they reach domain logic!",
        "Result<T> pattern handles errors without exceptions!",
        "MediatR Pipeline Behaviors add cross-cutting concerns (validation, logging)!",
        "The Application Layer is protocol-agnostic - pure business logic!",
        "InfrastructureLayer implements all external concerns (DB, auth, jobs)!",
        "Each layer has its own models - no leaky abstractions!",
        "Unit of Work pattern ensures transactional consistency!",
        "ConfigurationLayer provides centralized app settings with caching!",
        "The solution follows SOLID principles throughout!",
        "Domain Events could easily be added for event-driven architecture!",
        "Clean Architecture makes testing easy - mock any layer!",
        "The layered approach allows swapping implementations (REST ↔ gRPC)!",
        "No circular dependencies - dependency graph is acyclic!",
        "Each layer can be deployed independently if needed!"
    };

    public static readonly string[] ProtocolFacts = new[]
    {
        "REST uses HTTP/1.1 with JSON - human-readable and widely supported!",
        "SOAP uses XML over HTTP - enterprise-grade with WS-* standards!",
        "gRPC uses HTTP/2 with Protocol Buffers - 7x faster serialization!",
        "REST is stateless - each request contains all needed context!",
        "SOAP has built-in error handling with SOAP Faults!",
        "gRPC supports bi-directional streaming - true real-time!",
        "REST responses in this app use nested grouping: Provider → Base → Target!",
        "SOAP WSDL provides machine-readable service contracts!",
        "gRPC uses code generation from .proto files!",
        "All 3 protocols use JWT for authentication - unified security!",
        "REST SignalR provides WebSocket fallback for old browsers!",
        "gRPC Server Streaming keeps a persistent connection per client!",
        "REST endpoints return HTTP status codes (200, 401, 404)!",
        "SOAP always returns 200 OK - errors are in SOAP Body!",
        "gRPC uses status codes from Google's RPC spec!",
        "REST content negotiation supports JSON/XML (though we use JSON)!",
        "SOAP envelopes have Header (metadata) and Body (payload)!",
        "gRPC reflection allows tools like grpcurl to discover services!",
        "REST is cacheable - responses can be cached by proxies!",
        "gRPC multiplexes multiple calls over single TCP connection!"
    };

    public static readonly string[] TechnologyFacts = new[]
    {
        "This app uses .NET 9.0 - the latest LTS version!",
        "Entity Framework Core 9.0 provides the ORM layer!",
        "Hangfire schedules background jobs for daily rate fetches!",
        "SQLite in-memory database for development/demo mode!",
        "SQL Server support for production deployments!",
        "BCrypt hashes passwords with cost factor 11!",
        "JWT tokens expire after 60 minutes for security!",
        "SignalR uses WebSockets with automatic fallback!",
        "The app fetches rates from 3 providers: ECB, CNB, BNR!",
        "Historical data defaults to last 90 days!",
        "Database views provide optimized read queries!",
        "Hangfire Dashboard lets you monitor background jobs!",
        "The solution uses C# 12 features (primary constructors, etc.)!",
        "Async/await used throughout for scalability!",
        "Connection pooling optimizes database performance!",
        "The app supports both Admin and Consumer roles!",
        "CORS is configured for cross-origin requests!",
        "Swagger/OpenAPI documents the REST API!",
        "gRPC reflection enables Postman integration!",
        "Rate limiting protects APIs from abuse (configurable)!"
    };

    public static readonly string[] PerformanceFacts = new[]
    {
        "gRPC is typically 30-50% faster than REST for the same operation!",
        "Protocol Buffers are 3-10x smaller than JSON!",
        "HTTP/2 multiplexing eliminates head-of-line blocking!",
        "SignalR WebSockets reduce latency vs polling!",
        "Database connection pooling reduces connection overhead!",
        "EF Core query caching improves repeated queries!",
        "Async operations don't block threads - better scalability!",
        "In-memory database enables instant startup for demos!",
        "Hangfire uses polling with back-off for efficiency!",
        "JWT tokens avoid database lookups on each request!",
        "Repository pattern caching could cache frequent queries!",
        "gRPC streaming uses a single connection for all updates!",
        "REST pagination (PageSize/PageNumber) limits data transfer!",
        "SOAP MTOM can optimize binary data transfer!",
        "HTTP/2 server push could pre-send related resources!",
        "Database indexes speed up currency/provider lookups!",
        "Lazy loading in EF Core defers unnecessary data loads!",
        "Compiled queries in EF Core improve execution speed!",
        "Hangfire's SQL Server storage is production-optimized!",
        "Response compression reduces network bandwidth usage!"
    };

    public static readonly string[] DataFacts = new[]
    {
        "ECB (European Central Bank) uses EUR as base currency!",
        "CNB (Czech National Bank) provides CZK exchange rates!",
        "BNR (Banca Națională a României) reports RON rates!",
        "Exchange rates typically update once daily at specific times!",
        "ECB updates at 16:00 CET - after European markets close!",
        "CNB updates at 14:30 CET daily!",
        "BNR updates at 13:00 EET (Eastern European Time)!",
        "Rates include multiplier (e.g., 1 EUR = 25.34 CZK)!",
        "Historical rates enable trend analysis and reporting!",
        "The app stores ValidDate for each rate snapshot!",
        "Duplicate rates (same date/currencies) are rejected!",
        "Failed fetches are logged with retry mechanism!",
        "Consecutive failures trigger provider health alerts!",
        "Providers can be dynamically enabled/disabled!",
        "Rate retention period is configurable (default: 365 days)!",
        "Fetch logs track success/failure for each attempt!",
        "Error logs capture exceptions with stack traces!",
        "System configuration supports live updates without restart!",
        "Provider-specific settings stored in configuration table!",
        "Database constraints ensure data integrity!"
    };

    public static string GetRandomFact(Random random)
    {
        var allFacts = ArchitectureFacts
            .Concat(ProtocolFacts)
            .Concat(TechnologyFacts)
            .Concat(PerformanceFacts)
            .Concat(DataFacts)
            .ToArray();

        return allFacts[random.Next(allFacts.Length)];
    }

    public static int TotalFactCount =>
        ArchitectureFacts.Length +
        ProtocolFacts.Length +
        TechnologyFacts.Length +
        PerformanceFacts.Length +
        DataFacts.Length;
}
