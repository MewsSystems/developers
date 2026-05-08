# Decision Notes

## Framework

- Chose **.NET 10** as a current, supported runtime.
- Kept the implementation simple — no AOT, trimming, or advanced resilience patterns.

## Data Source

- **Czech National Bank (CNB) daily fixing** — official, public, no API key, plain-text format.
- Alternatives (ECB, Fixer.io) add complexity (XML, auth, rate limits).

## Architecture Notes

### Separation: fetch vs parse vs orchestration

The solution is split into three responsibilities:

- **`CnbRatesSource`**: fetches raw text from CNB (I/O, network concerns).
- **`CnbRatesParser`**: parses raw text into domain objects (pure logic).
- **`ExchangeRateProvider`**: orchestrates the flow and applies the task rules (filtering, CZK requirement).

This keeps parsing testable without HTTP and keeps the provider focused on business rules.

### Why `IExchangeRatesSource` exists

`IExchangeRatesSource` abstracts the external dependency (HTTP fetch).

Benefits:
- Unit tests can supply a fake source (`FakeRatesSource`) without network calls.
- The provider stays deterministic and easy to test (no flakiness, no timeouts).
- If the data source changes (CNB format endpoint, alternative provider), the provider logic stays the same.

### Why the parser is `static`

`CnbRatesParser` is a pure, stateless function. Making it static:
- communicates "no state, no side effects"
- avoids unnecessary DI wiring
- keeps unit tests focused and simple

If in the future parsing becomes configurable (different formats/sources), it can be converted to an injected service.

### Why the provider requires `CZK` in the request

CNB daily fixing publishes rates **against CZK** (e.g., `USD -> CZK`), not arbitrary pairs.

Requiring CZK in the input:
- makes it explicit that CZK is the target currency for returned results
- avoids returning confusing partial results when users ask for pairs CNB cannot provide

### Why no inverse/cross rates are computed

The assignment explicitly requires returning only rates defined by the source.

Computing inverse rates (`CZK -> USD`) or cross rates (`USD -> EUR`) would introduce derived values and rounding differences vs official CNB fixing.

### Sync-over-async choice

The console app is intended to run once and exit. Using a synchronous API simplifies usage (`GetExchangeRates(...)` returning `IEnumerable<ExchangeRate>`).

The HTTP call uses `GetAwaiter().GetResult()` which is acceptable here because:
- there is no UI thread / request context
- the process is short-lived

In a long-running service, I would expose `async` and use `IHttpClientFactory`.

## Error Handling

- Network errors and timeouts throw `InvalidOperationException` with URL for diagnostics.
- Fail-fast is appropriate for a console app; retries/circuit breakers omitted.

## Not Implemented

- Caching (stateless is simpler for a one-shot app)
- Retry policies / circuit breakers
- Integration tests against live CNB endpoint
- Historical rates
