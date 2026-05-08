# ExchangeRateUpdater

## Overview
A .NET 8 project that fetches daily exchange rates from the **Czech National Bank**, with:
- `IDistributedCache` support (Memory in dev, Redis in prod)
- XML parser with models
- REST API with Swagger
- Unit tests (xUnit + FluentAssertions + NSubstitute)
- Docker + docker-compose

---

## Run Locally

```bash
dotnet build
dotnet run --project ExchangeRateUpdater
