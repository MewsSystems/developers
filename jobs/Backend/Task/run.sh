#!/usr/bin/env bash
set -e

if [[ "$1" == "-v" || "$1" == "--verbose" ]]; then
    env Logging__LogLevel__ExchangeRateUpdater=Debug \
        dotnet run --project ExchangeRateUpdater.csproj
else
    dotnet run --project ExchangeRateUpdater.csproj
fi
