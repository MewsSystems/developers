# Engineering Decision Log

This file records lightweight technical decisions made during this task.

## CNB daily rates URL in `appsettings.json` and HTTP via `IHttpClientFactory`

### Context

`CnbExchangeRateSource` needs the public CNB daily rates document URL. We also want to avoid creating a new `HttpClient` on every call (socket churn and related issues).

| Decision | Why this choice | Consequences |
| --- | --- | --- |
| Store the CNB daily kurz URL in [`Task/appsettings.json`](../Task/appsettings.json), read through `IConfiguration`. | Separates environment-specific or future URL changes from compiled logic and matches common .NET hosting patterns. | The console app composition root ([`Task/Program.cs`](../Task/Program.cs)) should build configuration and bind/pass settings into `CnbExchangeRateSource`. |
| Use `IHttpClientFactory` via `AddHttpClient` to obtain `HttpClient` instances. | This is the recommended default for production-style .NET apps and addresses lifetime concerns that a per-call `new HttpClient()` does not. | Additional NuGet packages are expected for configuration and HTTP client extensions, such as `Microsoft.Extensions.Configuration.Json`, `Microsoft.Extensions.Http`, and hosting/DI primitives as needed. |

## Generic host for console bootstrap (`Host.CreateApplicationBuilder`)

### Context

The Task executable needs JSON configuration (e.g. CNB URL), options binding (`Configure<CnbOptions>`), and `AddHttpClient` registration. The main alternative is a **manual** stack: `ConfigurationBuilder` + `ServiceCollection` without a host.

| Decision | Why this choice | Consequences |
| --- | --- | --- |
| Bootstrap the console app with `Host.CreateApplicationBuilder(args)`. | It gives one conventional pipeline for default `appsettings` loading, environment-specific files, configuration, and DI. | Add `Microsoft.Extensions.Hosting` and `Microsoft.Extensions.Http` to the Task project; implement [`Task/Program.cs`](../Task/Program.cs) around the host builder instead of only `new`ing types in `Main`. |
| Use `builder.Configuration` and `builder.Services` for options, HTTP clients, and DI registrations, then `Build()` and resolve services. | This uses less boilerplate than assembling `ConfigurationBuilder`, `Build()`, and `ServiceCollection` by hand for the same features. | `Configure<CnbOptions>(builder.Configuration.GetSection(...))` and related registrations live next to other `builder.Services` calls in the composition root. |
| Do not use only a manual `ServiceCollection` + `ConfigurationBuilder` stack for this task. | The manual stack is valid when minimizing dependencies or when another host already owns configuration/DI, but that is not needed here. | The project follows the generic-host style consistently. |

## Use `Microsoft.Extensions.Options` (`IOptions<CnbOptions>`), not a custom `IOptions` type

### Context

It is tempting to add a small project-local interface (e.g. a non-generic `IOptions` with `DailyKurzUrl`) to abstract configuration for [`CnbExchangeRateSource`](../Task/CnbExchangeRateSource.cs).

| Decision | Why this choice | Consequences |
| --- | --- | --- |
| Do not introduce a custom `IOptions` or similarly named interface in the Task project. | `IOptions` and `IOptions<T>` are standard .NET names; a local `IOptions` type would collide conceptually and confuse readers, docs, and `using` resolution. | [`CnbExchangeRateSource`](../Task/CnbExchangeRateSource.cs) should not take a hand-rolled options interface. |
| Inject `IOptions<CnbOptions>` from `Microsoft.Extensions.Options`, bind settings with `Configure<CnbOptions>(...)`, and read the URL via `options.Value`. | `Configure<CnbOptions>` is already the chosen wiring, and `IOptions<CnbOptions>` is the intended consumer API for that binding. | [`CnbExchangeRateSource`](../Task/CnbExchangeRateSource.cs) should take `IOptions<CnbOptions>` plus `HttpClient` from `IHttpClientFactory`. |
| If extra indirection is needed later, use a distinct custom name such as `ICnbSettings`. | A distinct name avoids overlap with the standard `IOptions<T>` pattern. | Remove or avoid any obsolete `IOptions.cs` in [`Task`](../Task) that duplicates this role. |

## Composition root in `Program.cs` (no default `ExchangeRateProvider` ctor)

### Context

`ExchangeRateProvider` depends on `IExchangeRateSource`. A common shortcut is a parameterless constructor that chains to `new CnbExchangeRateSource()` (or similar) so callers can write `new ExchangeRateProvider()` without wiring.

| Decision | Why this choice | Consequences |
| --- | --- | --- |
| Do not add a parameterless `ExchangeRateProvider()` that internally `new`s a concrete source. | Dependencies stay visible at the application entry point, and `ExchangeRateProvider` avoids hard-coding a single concrete source implementation. | Every runnable entry point must create or receive an `IExchangeRateSource` before constructing `ExchangeRateProvider`. |
| Treat [`Program.cs`](../Task/Program.cs) as the composition root. | This aligns with explicit DI-style composition without pulling in a full container for this small task. | Unit tests continue to inject fakes via the same constructor; no convenience ctor is required for production. |
| Do not use a parameterless constructor that delegates to `new CnbExchangeRateSource()`. | That shortcut would reduce lines in `Program`, but it hides the dependency and couples the provider to one default implementation. | The provider remains source-agnostic and easier to test. |

## CNB source owns CNB-specific parsing

### Context

`IExchangeRateSource` could either return the raw CNB daily rates text (for example `Task<string>`) and leave parsing to `ExchangeRateProvider`, or return normalized `ExchangeRate` objects after handling the source-specific document format.

| Decision | Why this choice | Consequences |
| --- | --- | --- |
| `IExchangeRateSource` returns parsed `ExchangeRate` objects. | Avoiding `Task<string>` in `ExchangeRateProvider` prevents the provider from becoming coupled to one source's transport format and keeps its responsibility focused on orchestration and filtering. | Provider tests should fake `IExchangeRateSource` by returning `ExchangeRate` instances, not raw text snippets. |
| `CnbExchangeRateSource` owns both fetching the CNB daily document and parsing the CNB-specific text format. | The CNB document shape, including header rows, pipe-delimited columns, comma/dot decimal handling, and amount/rate normalization, is specific to the CNB implementation. | CNB parsing tests belong with `CnbExchangeRateSource` and can use stubbed HTTP responses. |
| `ExchangeRateProvider` receives already-normalized rates from `IExchangeRateSource`. | Keeping parsing in the concrete source keeps `ExchangeRateProvider` generic: it can orchestrate source -> filter -> return without knowing about CNB text files. | If another bank/source is added later, it should get its own implementation that fetches and parses its own format into `ExchangeRate` objects. |
| Keep parser helpers such as `ParseCnbDailyKurz` non-public. | Public members should describe supported production behavior, not expose implementation details solely for test access. | Parsing coverage should exercise `CnbExchangeRateSource.GetExchangeRates(...)` with controlled HTTP responses instead of calling `ParseCnbDailyKurz` directly. |
| Do not introduce a parser interface for now. | A parser interface would add ceremony before there are multiple parsers or a need to test parsing independently from the source class. | Revisit this only if multiple parsers or a separate parsing lifecycle appear. |

## Use xUnit for unit tests

### Context

We need a first unit-test setup for the .NET backend task and must choose a test framework.

| Decision | Why this choice | Consequences |
| --- | --- | --- |
| Use `xUnit` as the default unit testing framework for `Task.Tests`. | It has strong and common ecosystem support in modern .NET projects, a simple `[Fact]` / `[Theory]` model, smooth `dotnet test` integration, and minimal setup overhead for a small codebase. | Test examples and conventions in this repository should assume xUnit attributes and assertions. |
| Prefer xUnit's constructor-based setup and data-driven tests for this task. | Constructor-based setup encourages explicit dependencies and cleaner tests; `[Theory]` plus data attributes gives strong parameterized test support. | If a team is historically NUnit/MSTest-heavy, migration has small friction because lifecycle patterns differ. |
| Do not choose NUnit for this task. | NUnit is mature, feature-rich, familiar to many teams, and has strong parameterized testing, but there is no project-specific reason to prefer it here. | Avoids another style divergence; extra NUnit features are not required for this small task. |
| Do not choose MSTest for this task. | MSTest is Microsoft-native and enterprise-friendly, but it is usually more verbose for simple behavior-driven tests and less commonly preferred in modern community samples. | If we later need richer fluent assertions or mocking, we can add packages without changing the test framework. |

## Keep `ExchangeRateProvider.GetExchangeRates` synchronous

### Context

The assignment-facing API already exposes `ExchangeRateProvider.GetExchangeRates(...)` as a synchronous method returning `IEnumerable<ExchangeRate>`. The concrete CNB source uses HTTP, where the natural .NET API is asynchronous.

| Decision | Why this choice | Consequences |
| --- | --- | --- |
| Keep `ExchangeRateProvider.GetExchangeRates(...)` synchronous for this task. | It preserves the public provider API expected by the exercise and existing callers. | `ExchangeRateProvider` blocks while waiting for the source result. |
| Keep `IExchangeRateSource.GetExchangeRates(...)` asynchronous so the source can use async HTTP APIs. | It keeps HTTP-specific async behavior inside the source abstraction instead of pushing it through the whole console app for this small task. | This is acceptable for the current console assignment, but a server or high-concurrency app should prefer an async provider API such as `GetExchangeRatesAsync`. |
| Use a deliberate sync-over-async boundary inside `ExchangeRateProvider` when calling the source. | The boundary is explicit and localized, so it can be changed later if the public API moves to async. | If the task evolves to support a fully async public surface, update `Program.cs`, provider tests, and this decision. |

## Keep requested-currency filtering in `ExchangeRateProvider`

### Context

`IExchangeRateSource.GetExchangeRates(...)` accepted the requested currencies even though the current source design returns all parsed CNB rows and the provider filters them afterward.

CNB publishes rates as foreign currency against CZK, so `CZK` is implicit in each parsed rate such as `USD/CZK`.

| Decision | Why this choice | Consequences |
| --- | --- | --- |
| Make `IExchangeRateSource.GetExchangeRates()` parameterless. | The source contract stays focused on fetching and parsing source-provided rates. | Source implementations cannot selectively fetch or filter by requested currencies through the interface. |
| Keep the requested `Currency` collection on `ExchangeRateProvider.GetExchangeRates(...)`, where filtering happens. | `ExchangeRateProvider` remains the single place that applies the assignment's requested-currency filtering rules. Avoids an unused parameter in `CnbExchangeRateSource` and fake source implementations. | If a future source needs request-aware fetching for performance, revisit the source contract deliberately. |
| Treat requested currencies as requested source currencies; `USD` is enough to return the source-provided `USD/CZK` rate. | This matches CNB's real data format and avoids requiring callers to pass `CZK` redundantly when every CNB rate is already against CZK. | This assumes the source's target currency is implicit. If a future source supports multiple target currencies, revisit the provider API, for example with `sourceCurrencies + targetCurrency` or explicit currency pairs. |

## Configure CNB HTTP resilience

### Context

The CNB source calls an external HTTP endpoint. In production-style code, a stalled or transiently failing network call should be bounded and retried deliberately instead of relying on one fixed `HttpClient.Timeout` value.

| Decision | Why this choice | Consequences |
| --- | --- | --- |
| Store CNB resilience settings in [`Task/appsettings.json`](../Task/appsettings.json): total timeout, per-attempt timeout, retry count, and retry delay. | These values are operational knobs, not parsing or domain logic. Keeping them in configuration lets environments tune CNB behavior without code changes. | [`Task/CnbOptions.cs`](../Task/CnbOptions.cs) now carries both the CNB URL and resilience settings. |
| Validate the CNB URL and resilience settings in [`Task/Program.cs`](../Task/Program.cs) before registering the HTTP client. | A missing URL or invalid timeout should fail with a clear configuration error instead of becoming a late HTTP or resilience-pipeline failure. | The console app reports configuration mistakes at startup through the existing top-level error handling. |
| Use `Microsoft.Extensions.Http.Resilience` with `AddStandardResilienceHandler` on the typed CNB `HttpClient`. | Retries and timeouts belong at the HTTP boundary. The standard handler provides a production-ready pipeline for retries, attempt timeout, total timeout, and jittered exponential backoff without a hand-written retry loop. | [`Task/Program.cs`](../Task/Program.cs) configures the resilience policy; [`Task/CnbExchangeRateSource.cs`](../Task/CnbExchangeRateSource.cs) remains focused on fetching through `HttpClient` and parsing CNB text. |
| Set `HttpClient.Timeout` to `Timeout.InfiniteTimeSpan` and let the resilience pipeline own request timing. | Stacking `HttpClient.Timeout` on top of resilience timeouts can produce competing cancellation behavior and harder-to-explain failures. | Timeout failures are controlled by the configured attempt and total request timeouts. |

## Target .NET 10 LTS

### Context

The backend task should look like something we would maintain long term. The projects previously targeted .NET 9, which was a short-term support release.

| Decision | Why this choice | Consequences |
| --- | --- | --- |
| Target `net10.0` for both the console app and test project. | .NET 10 is the current LTS line, so it is a better match for the task's production-maintenance framing than .NET 9 STS. | Local and reviewer machines need the .NET 10 SDK installed to restore, build, and test. |
| Align `Microsoft.Extensions.Hosting` and `Microsoft.Extensions.Http` package references to the latest stable 10.x packages selected by NuGet. | Keeping extension packages on the same major version as the target framework avoids unnecessary version skew. | Package restore now depends on the 10.x Microsoft.Extensions package line. |
| Add `Microsoft.Extensions.Http.Resilience` for the CNB HTTP policy. | This is the standard .NET HTTP resilience package and avoids maintaining custom retry code. | Restore also brings the related Microsoft resilience and diagnostics packages. |

## Project docs and AI collaboration defaults

### Context

We want consistent planning and decision history, and a predictable way to collaborate with AI assistants (step-by-step discussion, no silent architectural choices).

| Decision | Why this choice | Consequences |
| --- | --- | --- |
| Keep the versioned implementation plan in [`PLAN.md`](PLAN.md) under `jobs/Backend/docs` and update it when the plan changes. | `PLAN.md` is reviewable in git and independent of IDE-generated plan files. | When scope or design shifts, edit `PLAN.md`. Optional Cursor plans under `.cursor/plans/` may still exist; treat `PLAN.md` as authoritative unless the team agrees otherwise. |
| Record substantive technical choices in [`DECISIONS.md`](DECISIONS.md). | `DECISIONS.md` stays the single place for why we chose a given approach. | Add a short entry here when a shift reflects a real decision, such as libraries, new types, or major tradeoffs. |
| Encode collaboration preferences in [`.cursor/rules/collaboration-and-docs.mdc`](../../../.cursor/rules/collaboration-and-docs.mdc). | Cursor rules give stable defaults for future sessions without repeating instructions. | Future AI sessions should work step by step and avoid silent architectural choices. |

## No root `AGENTS.md` (for now)

### Context

We considered adding a root-level `AGENTS.md` for discoverability and cross-tool conventions, versus relying only on Cursor project rules and backend docs.

| Decision | Why this choice | Consequences |
| --- | --- | --- |
| Do not add a repository root `AGENTS.md` at this stage. | Avoids duplication and drift between `AGENTS.md` and `.cursor/rules` if both repeated the same instructions. | Anyone looking for agent collaboration guidance should open `.cursor/rules/` and the backend `docs/PLAN.md` / `docs/DECISIONS.md` files. |
| Treat [`.cursor/rules/collaboration-and-docs.mdc`](../../../.cursor/rules/collaboration-and-docs.mdc), [`PLAN.md`](PLAN.md), and this [`DECISIONS.md`](DECISIONS.md) as authoritative. | Cursor's primary hook for persistent guidance is `.cursor/rules/`; a second root file does not add much for Cursor-only workflows on a small repo. | If we add `AGENTS.md` later, keep it as a short index of links unless we explicitly deprecate overlap with `.cursor/rules`. |
