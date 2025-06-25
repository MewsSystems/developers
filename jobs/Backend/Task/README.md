# BackEnd task for ExchangeRateUpdater

This program is designed in a way, that only in [ExchangeRateUpdate](ExchangeRateUpdater/ExchangeRateUpdater.csproj) is
a binary runnable program. Other projects are thought to be separate Nuget packages, so they can be reused freely in 
another applications.

## Requirements

To run this application, we need **.NET 8** and **c# 12**.

You can also run this with Docker, if you don't have .net8 installed. Just run:

```bash
docker run -it --rm -v ./:/app -w /app mcr.microsoft.com/dotnet/sdk:8.0 /bin/bash
```
**Make sure that the mounted volume is the correct path to the BackEnd task folder**

## Run

To run the CLI just run:

```bash
dotnet restore
dotnet run --project ExchangeRateUpdater/ExchangeRateUpdater.csproj
```

This will run the [Program.cs](ExchangeRateUpdater/Program.cs). It's a basic CLI that asks for target currency and date
for which we want to know the exchange rate and prints out the correct rate.

The purpose of this program is to show one possible usage.

## Test

To run all test:

```bash
dotnet restore
dotnet test
```

There are some unit tests testing the main functionality of this application.
Also, there is a E2E test for provider, so we can check how the components work together.