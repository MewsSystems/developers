# Exchange Rate Provider

Program to fetch exchange rates from Czech National Bank (CNB) and lists only _selected_ ones.

All listed exchange rates are in relation to the Czech crown (CZK).

## Prerequisites

- [net8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- connection to [the internet](https://www.youtube.com/watch?v=iDbyYGrswtg)

## Execute

Before running the program, edit [`Program.cs`](ExchangeRateUpdater/Program.cs) and set the `Program.Currencies`
variable to the currencies you want to list.

```bash
dotnet run --project ExchangeRateUpdater
```

## Test

It is possible to run unit tests and/or integration tests separately or all together.

```bash
dotnet test --filter Category=Unit
dotnet test --filter Category=Integration
dotnet test
```

## Benchmarks

Project contains few benchmarks (namely to compare algorithms of selecting currencies by [`ExchangeRateTransformer`](ExchangeRateUpdater/ExchangeRateProvider.cs).
To run *all* benchmarks, use following command (if executing from IDE, make sure you switch to `Release` configuration):

```bash
dotnet run -c Release --project ExchangeRateUpdater.Benchmarks -- --filter *
```
