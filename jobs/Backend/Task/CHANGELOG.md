# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com),
and this project adheres to [Semantic Versioning](https://semver.org).

## [Unreleased]

### Added

### Fixed

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
