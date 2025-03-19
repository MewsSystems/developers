Exchange Rate Updater

Overview

This project is a production-ready exchange rate updater that fetches exchange rates from an external API. The implementation follows best practices in observability, resilience, and performance optimization to ensure reliability in a real-world environment.

Observability & Telemetry

Since the task required a production-ready system, observability was a key consideration. A service in production must provide insights into its behavior, performance, and failures.

To achieve this, OpenTelemetry has been integrated to track requests, measure execution times, and log retries and failures. Without observability, debugging issues in production would be difficult, making monitoring and alerting essential for maintaining system health and performance.

Health Checks

The application includes health checks to monitor the system’s status and dependencies. Health checks:

Verify that the service is running.

Ensure connectivity with external dependencies.

Provide an endpoint (/health) that can be used for monitoring (e.g., Kubernetes liveness/readiness probes).

A custom response writer was implemented to return structured JSON data, allowing observability tools to parse the health status efficiently.

Caching Strategy

API calls to third-party services can be costly in terms of both performance and rate limits. To minimise unnecessary API requests, caching has been implemented using an in-memory cache. The cache:

Stores exchange rates for a fixed duration (1 hour) to reduce the load on the external API.

Improves response time by serving data from memory instead of making network calls.

Prevents excessive requests that could lead to API rate limiting or additional charges.

In a real-world production environment, Redis would be used instead of in-memory caching to support multiple instances of the service and ensure a distributed caching strategy. This would improve scalability and consistency across deployments.

Caching should always be considered when dealing with external APIs to improve efficiency and resilience.

Testing Strategy

Unit tests were added to cover the critical parts of the system:

ExchangeRateService: Ensures API requests, caching behavior, and error handling work correctly.

Error Handling & Logging: Verifies that failed requests log appropriate error messages.

Caching Behavior: Confirms that cached data is used when available.

What was not covered:

DTOs were not explicitly tested, as their sole purpose is to store data.

External integrations (e.g., actual API calls) were mocked to avoid network dependencies in tests.

Licensing Considerations

This project uses FluentAssertions v8.x for testing, which now requires a commercial license for production use. If this were to go to production, a downgrade to FluentAssertions v7.x or an alternative assertion library (e.g., Shouldly) would be necessary to avoid licensing costs.

Being aware of these changes ensures that we remain cost-conscious and compliant with licensing requirements in a production environment.

Conclusion

This project was built with production-readiness in mind, incorporating observability, resilience, and performance optimisation. The integration of OpenTelemetry, Polly for retries, caching, and structured logging ensures that the system can handle real-world usage reliably.