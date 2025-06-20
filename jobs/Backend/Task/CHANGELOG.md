# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com),
and this project adheres to [Semantic Versioning](https://semver.org).

## [Unreleased]

### Added
- 2 HttpClients which fetch the Czech National Bank data sources
- Parser for parsing data from the http call
- Integration Test testing entire flow with mocked Http Responses
- The `CHANGELOG` file itself
- `Deployment.md` and `Dockerfile` for deployment steps

### Fixed
- Single Responsibility Principle was being violated by the `ExchangeRateProviderService`
- Moved the `ExchangeRateProvider` to Services folder keeping root clean
- Unit tests which were failing from the new `ExchangeRateProvider` implementation
- csproj organisation and using .net 9 packages on a project with target of .net 6
- `appsettings` to folow the removal of `HttpServiceSettings`

### Removed
- `HttpServiceSettings`; moved into the `CzechBankSettings`

---

## [1.0.0] - 2025-05-22

### Added
- Initial implementation of `ExchangeRateProvider`
- Fetch rates from CNB daily and other endpoints
- Dependency injection setup in console app
- Logging and configuration via `appsettings.json`
- HTTP timeout and retry support via Polly, configurable from appsettings
- Unit tests for `ExchangeRateProvider`
- Structured logging using Microsoft.Extensions.Logging
- Cache fallback logic

### Fixed
- PlatformNotSupportedException from EventLog on non-Windows systems
- ArgumentOutOfRangeException for timeout value
