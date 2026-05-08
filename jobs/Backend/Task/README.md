# Exchange Rate Updater

A .NET 10 console application that fetches daily foreign currency exchange rates from the Czech National Bank (CNB). It filters rates by a configurable list of currencies and outputs them to the console.

## Behavior Rules

- **Source-defined rates only**: Returns only rates published by CNB (e.g., `EUR -> CZK`).
- **No inverse rates**: `CZK -> EUR` is NOT computed as `1 / rate`.
- **No cross rates**: `USD -> EUR` is NOT derived via `USD -> CZK -> EUR`.
- **CZK required**: CZK must be in the requested currency list (it is the target currency for all rates).
- **Missing currencies ignored**: Unknown or unavailable currency codes are silently skipped.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## How to Run

```bash
./run.sh          # default (summary output)
./run.sh -v       # verbose (debug logs)
```

## How to Test

```bash
./test.sh
```

## Configuration

| Method | Key | Default |
|--------|-----|---------|
| appsettings.json | `CnbApi:BaseUrl` | `https://www.cnb.cz` |
| Environment variable | `CnbApi__BaseUrl` | — |

## Design Decisions

See [DECISIONS.md](DECISIONS.md).
