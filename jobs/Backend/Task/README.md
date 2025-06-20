# CzechNationalBank ExchangeRateUpdater

A robust .NET 6 console application that pulls daily and other currency exchange rates from the Czech National Bank, parses and caches them, and exposes a reusable provider for integration into other systems.

---

## Features

- ✅ Fetches official CNB daily and other currency exchange rates
- ✅ Handles parsing of CNB's custom text formats
- ✅ Uses resilient HTTP client with Polly retry and timeout policies
- ✅ Caches rates until the next official update (2:30 PM daily)
- ✅ Fully unit-tested and integration-tested
- ✅ Runs standalone or inside a Docker container

---

## Getting Started

### Requirements

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional)

---

### Build & Run Locally

`bash`
# Build & run
dotnet build
dotnet run --project ExchangeRateUpdater

---

## Run with Docker

### Publish build
dotnet publish -c Release -o ./publish

### Build Docker image
docker build -t exchangerate-updater .

### Run in Docker
docker run --rm exchangerate-updater

--- 

# Configuration
The app reads from `appsettings.json`

Override these settings at runtime with env variables eg:

`docker run --rm -e CzechBankSettings__DailyRatesUrl=https://mock exchangerate-updater`

---

## Run Tests

`dotnet test ExchangeRateUpdaterTests`

---

## Deployment

See [deployment.md](./deployment.md) for detailed instructions

---

## Changelog

[Changelog](./CHANGELOG.md)