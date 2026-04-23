## Summary

I opted for an API architecture to provide a foundation for integrating multiple exchange rate providers in the future.
This enables the API to serve as a more than just a "proxy" for another provider's API. In a realistic scenario, future
iterations could include multi-provider aggregation/comparison of exchange rates, as well as allowing consumers to fall
back to alternative providers in case of temporary outages.

## Features, considerations and future improvements

### Extensibility

The architecture is reasonably generic, meaning that additional providers can be easily integrated. This could be taken
a few steps further, but I felt that this was sufficient for the purposes of this test.

### Containerization, IaC, deployment pipeline, CI/CD

The solution uses docker/docker-compose to spin up the API and Seq (for telemetry).

For a more production-ready solution, I would aim to use the following cloud stack:

- AKS
- Provisioning of resources via code using Bicep or Terraform
- Multi-stage pipelines via ADO or GitHub actions

### Security

Security was out-of-scope for this exercise, particularly due to the fact that this is a public API integration and no
authentication or authorization was required. However, in a deployed scenario, I would endeavour to at least use HTTPs.
Additionally, storing and loading secrets/keys would be facilitated via a secure store such as Azure Key Vault.

### Monitoring, alerting and observability

The solution is set up to use OpenTelemetry and Serilog for basic instrumentation, which is then fed
to [Seq](https://datalust.co/seq). Seq runs as part of the docker container and provides
a [dashboard](http://localhost:5341/#/events).

In a more mature solution, I would instead consider:

- Using cloud-hosted solutions such as App Insights/Prometheus/Grafana instead of self-hosting Seq for telemetry/logging
- Alerts for error burn-rate, unusual traffic, 3rd party API downtime alerts, etc.
- Creating useful analytics dashboards

### Caching

I would have liked to incorporate caching to this API as a means of reducing repetitive calls to CNB. This would have
likely been done using Redis (spun up via docker/docker-compose, similarly to what I did with Seq).

### Unit/integration tests

The suite of unit and integration tests is intentionally not comprehensive, however it exercises the key happy/unhappy
paths.

### Performance tests

I decided that performance and degradation tests were out-of-scope for this exercise, though they would be worthwhile
adding. Performance tests would be particularly useful in environments where scaling and throughput
provisioning is self-managed.

### Validation

FluentValidation is used to validate incoming requests (replicating the rules of the CNB API). Note that
validation is configured to occur automatically for all matching requests using IEndpointFilter (a form of middleware).

### Contract testing/Open API

Although contract tests haven't been added, the API does
generate [Open API specs](http://localhost:5250/swagger/v1/swagger.json) (and also provides Swagger UI in dev mode).
Additionally, the integration tests exercise all routes with combinations of valid/invalid parameters. Ideally, I would
have written some PACT tests using PactNet, though admittedly this is an area I've had limited exposure to. I would
presumably aim PACT test both external APIs and my own.

### Rate limiting

Rate limiting has been added to avoid DoSing CNB. This was configured using the default
Microsoft.Extensions.Resilience/Poly rate limiting policy.

### Resilience

A basic level of resilience has been added with the use of Microsoft.Extensions.Resilience/Poly (automatic retries).

As a future improvement, automatic fallback could be useful when calls to exchange rate providers are unsuccessful (e.g.
due to temporary outage). There could be an endpoint that isn't specific to a provider, which would return a generic
response with exchange rates from any of the available/configured providers.

### Notes

- I have opted not to use nullable reference types as I am still experimenting with them in my own time. I did consider
  using Option<>, but decided to keep it simple.
- The Swagger/OTEL specs that are being generated are missing annotations such as required, examples, description
  etc. This is a limitation in .NET 8 (specifically minimal APIs) which will be fixed in .NET 10.
- A manual trace log is being performed in the `CzechNationalBankClient`. This is something that I'd normally do with
  the help of middleware (or configure the instrumentation SDK to include this).
- Note that I deliberately inject HttpClient as opposed to an HttpClientFactory. This is because
  [typed client registration](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0#typed-clients)
  in .NET 8 now handles the management of HttpClient lifecycles behind the scenes.

## Instructions

### Running the API

#### Docker

1. Ensure that the docker engine is running (targeting Linux, not Windows)
2. In a terminal, navigate to the directory where the solution is located
3. Execute `docker-compose up`

#### Rider/Visual Studio

1. Ensure that the `ExchangeRateUpdater.Api` is set as the Startup project
2. Press F5 to build and launch

### Testing

1. An `ExchangeRateUpdater.Api.http` file is provided in the `ExchangeRateUpdater.Api` project's directory. This
   includes a suite of requests (OK and BadRequest) that can be made against the API.
2. Unit and Integration Test projects are present in the solution. Execute them using your IDE's test runner UI, or by
   running `dotnet test` in the directory where the .sln file is located. Note that it is not necessary for the API to
   be running in order for the integration tests to succeed (the API is automatically spun up in memory).
3. View traces/metrics in the [Seq Dashboard](http://localhost:5341/#/events)