FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# copy csproj files for restore caching
COPY ExchangeRateUpdater.Worker/ExchangeRateUpdater.Worker.csproj ExchangeRateUpdater.Worker/
COPY ExchangeRateUpdater.Application/ExchangeRateUpdater.Application.csproj ExchangeRateUpdater.Application/
COPY ExchangeRateUpdater.Infrastructure/ExchangeRateUpdater.Infrastructure.csproj ExchangeRateUpdater.Infrastructure/
COPY ExchangeRateUpdater.Domain/ExchangeRateUpdater.Domain.csproj ExchangeRateUpdater.Domain/

RUN dotnet restore ExchangeRateUpdater.Worker/ExchangeRateUpdater.Worker.csproj

# copy everything else
COPY . .

WORKDIR /src/ExchangeRateUpdater.Worker
RUN dotnet build "ExchangeRateUpdater.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/ExchangeRateUpdater.Worker
RUN dotnet publish "ExchangeRateUpdater.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./

# Start the published worker DLL directly.
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Worker.dll"]
